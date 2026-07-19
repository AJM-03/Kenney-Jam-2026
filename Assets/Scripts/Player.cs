using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;
    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer rend;
    [HideInInspector] public CameraMovement cam;

    [SerializeField] float jumpForce = 4f;
    [SerializeField] float minDistance = 0.5f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float rayDistance = 1.1f;
    [SerializeField] float standHeight = 0.67f;
    [SerializeField] float minAirtime = 0.75f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask stickyGroundMask;


    [SerializeField] AudioSource jumpSound;
    [SerializeField] AnimationCurve jumpPitch;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource deathFallSound;
    [SerializeField] AudioSource wallHitSound;
    [SerializeField] AudioSource bounceSound;
    [SerializeField] AudioSource landingSound;
    [SerializeField] AudioSource stickyLandingSound;


    private Transform groundSurface;
    private Vector2 prevNormal;


    private bool isGrounded;
    private bool canJump;
    private bool jumpedFromGround;
    private float groundDetectionTime;
    private float airtime;
    private float wallHitTime;
    private bool isDragging = false;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 force;
    private float distance;
    private bool previewMode;
    private float prevPreviewY;
    private bool dead;





    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        cam = Camera.main.GetComponent<CameraMovement>();

        startPoint = Vector2.zero;
        GameManager.Instance.trajectory.StartPreview();
        previewMode = true;
    }

    public void Jump(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        anim.SetBool("IsJumping", true);
        jumpSound.Play();

        if (jumpedFromGround)
        {
            if (force.x > 0) rend.flipX = true;
            else rend.flipX = false;
        }
    }




    private void Update()
    {

        if (isGrounded) 
            canJump = true;

        else
        {
            airtime += Time.deltaTime;
            wallHitTime -= Time.deltaTime;
            DetectGround();

            if (!jumpedFromGround)
            {
                transform.up = rb.velocity;
            }
        }

        if (dead) return;

        if (canJump)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnDragEnd();
                isDragging = false;
            }

            if (isDragging)
            {
                OnDrag();
            }
        }

        if (previewMode) PreviewMode();
    }

    private void OnDragStart()
    {
        if (previewMode) 
        {
            GameManager.Instance.trajectory.EndPreview();
            previewMode = false;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        startPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        GameManager.Instance.trajectory.Show();
    }

    private void OnDrag()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        endPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.Log(endPoint);
        distance = Vector2.Distance(startPoint, endPoint);
        if (distance < minDistance) distance = minDistance;
        if (distance > maxDistance) distance = maxDistance;
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * jumpForce;

        Debug.DrawLine(startPoint, endPoint);

        GameManager.Instance.trajectory.UpdateDots(transform.position, force);
        GameManager.Instance.trajectory.UpdateChargeRotate(distance, force);
        Debug.Log(distance);
        cam.TriggerChargeShake(distance / 3);
        jumpSound.pitch = jumpPitch.Evaluate(distance / 3);
    }

    private void OnDragEnd()
    {
        if (previewMode) return;
        if (!isDragging) return;
        Unground();
        Jump(force);
        GameManager.Instance.trajectory.Hide();
    }


    public void Unground()
    {
        isGrounded = false;
        canJump = false;
        rb.isKinematic = false;
        groundSurface = null;
        transform.parent = null;
        groundDetectionTime = 0;
        airtime = 0;
        wallHitTime = 0;
    }

    public void Ground(Transform surface)
    {
        isGrounded = true;
        groundSurface = surface;
        transform.parent = groundSurface;
        rb.isKinematic = true;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
        anim.SetBool("IsJumping", false);
        jumpSound.Stop();
    }

    private void DetectGround()
    {
        if (airtime <= minAirtime) return;

        RaycastHit2D hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, rayDistance); // Visualize the contact point
        hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundMask);
        if (hit)
        {
            HitGround(hit);
            return;
        }

        Debug.DrawRay(transform.position + new Vector3(-0.5f, 0, 0), Vector3.down, Color.red, rayDistance); // Visualize the contact point
        hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0, 0), Vector2.down, rayDistance, groundMask);
        if (hit)
        {
            HitGround(hit);
            return;
        }

        Debug.DrawRay(transform.position + new Vector3(0.5f, 0, 0), Vector3.down, Color.red, rayDistance); // Visualize the contact point
        hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, rayDistance, groundMask);
        if (hit)
        {
            HitGround(hit);
            return;
        }


        groundDetectionTime = 0;
    }

    private void HitGround(RaycastHit2D hit)
    {
        groundSurface = hit.transform;
        //Debug.Log("Standing on: " + groundSurface.name);

        if (rb.velocity.magnitude < 0.85f)
        {
            groundDetectionTime += Time.deltaTime;
            if (groundDetectionTime >= 0f)
            {
                transform.rotation = Quaternion.identity;
                transform.position = new Vector3(transform.position.x, hit.point.y + standHeight, 0);
                jumpedFromGround = true;
                landingSound.Play();
                prevNormal = hit.normal;
                Ground(hit.transform);
            }
        }

        else groundDetectionTime = 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded) return;
        CollisionCheck(collision);

        if (!isGrounded && wallHitTime <= 0f && (((stickyGroundMask.value & (1 << collision.gameObject.layer)) != 0) || (groundMask.value & (1 << collision.gameObject.layer)) != 0))
        {
            if (collision.gameObject.tag == "Bounce")
            {
                bounceSound.Play();
                airtime++;
            }
            else
                wallHitSound.Play();

            wallHitTime = 1;
        }
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (isGrounded) return;
        CollisionCheck(collision);
    }


    private void CollisionCheck(Collision2D collision)
    {
        if ((stickyGroundMask.value & (1 << collision.gameObject.layer)) != 0)  // If colliding object is in the ground layer
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (airtime <= minAirtime && prevNormal == contact.normal) return;
                prevNormal = contact.normal;

                // Rotate to be standing on the surface
                float angle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                var vec = targetRotation.eulerAngles;
                vec.x = Mathf.Round(vec.x / 45) * 45;
                vec.y = Mathf.Round(vec.y / 45) * 45;
                vec.z = Mathf.Round(vec.z / 45) * 45;
                targetRotation.eulerAngles = vec;
                transform.localRotation = targetRotation;

                transform.position = contact.point + (standHeight * contact.normal);
                jumpedFromGround = false;
                stickyLandingSound.Play();
            }
            Ground(collision.transform);
        }
    }


    public void KillPlayer()
    {
        dead = true;
        Unground();
        jumpedFromGround = false;
        Jump(new Vector2(0, 4));
        GameManager.Instance.trajectory.Hide();
        deathSound.Play();
        deathFallSound.Play();
        col.enabled = false;
    }




    public void PreviewMode()
    {
        if (startPoint == Vector2.zero)
        {
            startPoint = new Vector2(3f, -5.5f);
            endPoint = startPoint;
            DOTween.To(() => endPoint, x => endPoint = x, new Vector2(5f, -7f), GameManager.Instance.trajectory.previewSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * jumpForce;
        GameManager.Instance.trajectory.UpdateDots(transform.position, force);
        GameManager.Instance.trajectory.UpdateChargeRotate(distance, force);

        if (GameManager.Instance.trajectory.previewCursor.transform.position.y > prevPreviewY)
        {
            GameManager.Instance.trajectory.previewRend.sprite = GameManager.Instance.trajectory.previewOpenSprite;
        }
        else
        {
            GameManager.Instance.trajectory.previewRend.sprite = GameManager.Instance.trajectory.previewGrabSprite;
            GameManager.Instance.trajectory.Show();
        }
        prevPreviewY = GameManager.Instance.trajectory.previewCursor.transform.position.y;
    }
}