using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    [SerializeField] float jumpForce = 4f;
    [SerializeField] float maxDistance = 5f;

    private Transform stuckSurface;

    private bool isGrounded;
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
            if (rb.velocity.magnitude < 0.1)
            {
                DeactivateRB();
            }
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
        if (distance > maxDistance) distance = maxDistance;
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * jumpForce;

        Debug.DrawLine(startPoint, endPoint);

        GameManager.Instance.trajectory.UpdateDots(transform.position, force);
    }

    private void OnDragEnd()
    {
        ActivateRB();
        Jump(force);
        GameManager.Instance.trajectory.Hide();
    }


    public void Unground()
    {
        isGrounded = false;
        rb.isKinematic = false;
        stuckSurface = null;
        transform.parent = null;
    }

    public void Ground()
    {
        isGrounded = true;
        //stuckSurface =
        transform.parent = stuckSurface;
        rb.isKinematic = true;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
    }

    private void DetectGround()
    {

    }
}
