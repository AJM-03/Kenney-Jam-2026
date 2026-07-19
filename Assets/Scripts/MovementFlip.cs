using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFlip : MonoBehaviour
{
    private SpriteRenderer rend;
    private float prevX;
    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > prevX) rend.flipX = true;
        else rend.flipX = false;
        prevX = transform.position.x;
    }
}
