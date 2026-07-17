using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Jump(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRB()
    {
        rb.isKinematic = false;
    }

    public void DeactivateRB()
    {
        rb.isKinematic = true;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
    }
}
