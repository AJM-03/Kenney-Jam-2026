using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    [SerializeField] float jumpForce = 4f;
    [SerializeField] float minDistance = 0.5f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float rayDistance = 1.1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask stickyGroundMask;

    private Transform groundSurface;
    private Vector2 prevNormal;


    private bool isGrounded;
    private float groundDetectionTime;
    private float airtime;
    private bool isDragging = false;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 force;
    private float distance;





    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Jump(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }






    private void Update()
    {
        if (isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnDragEnd();
            }

            if (isDragging)
            {
                OnDrag();
            }
        }

        else
        {
            airtime += Time.deltaTime;
            DetectGround();
        }
    }

    private void OnDragStart()
    {
        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        if (distance < minDistance) distance = minDistance;
        if (distance > maxDistance) distance = maxDistance;
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * jumpForce;

        Debug.DrawLine(startPoint, endPoint);

        GameManager.Instance.trajectory.UpdateDots(transform.position, force);
        GameManager.Instance.trajectory.UpdateChargeRotate(distance, force);
    }

    private void OnDragEnd()
    {
        Unground();
        Jump(force);
        GameManager.Instance.trajectory.Hide();
    }


    public void Unground()
    {
        isGrounded = false;
        rb.isKinematic = false;
        groundSurface = null;
        transform.parent = null;
        groundDetectionTime = 0;
        airtime = 0;
    }

    public void Ground(Transform surface)
    {
        isGrounded = true;
        groundSurface = surface;
        transform.parent = groundSurface;
        rb.isKinematic = true;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
    }

    private void DetectGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, rayDistance); // Visualize the contact point
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundMask))
        {
            groundSurface = hit.transform;
            Debug.Log("Standing on: " + groundSurface.name);

            if (rb.velocity.magnitude < 0.2)
            {
                groundDetectionTime += Time.deltaTime;
                if (groundDetectionTime >= 0.5f) Ground(hit.transform);
            }

            else groundDetectionTime = 0;
        }
        else groundDetectionTime = 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded) return;
        CollisionCheck(collision);
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
                Debug.Log(prevNormal + " " + contact.normal);
                if (airtime <= 1 && prevNormal == contact.normal) return;
                prevNormal = contact.normal;

                Debug.Log(contact.point + " " + contact.normal);

                // Rotate to be standing on the surface
                float angle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                transform.rotation = targetRotation;

                transform.position = contact.point + (0.7f * contact.normal);
            }
            Ground(collision.transform);
        }
    }
}