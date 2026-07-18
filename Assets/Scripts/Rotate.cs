using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Rotate : MonoBehaviour
{
    public Vector3 rotate;
    private Transform trans;

    private void Start()
    {
        trans = gameObject.transform;
    }

    void Update()
    {
        trans.Rotate(rotate *  Time.deltaTime);
    }
}
