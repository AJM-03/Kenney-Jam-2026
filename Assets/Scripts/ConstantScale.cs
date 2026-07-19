using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScale : MonoBehaviour
{
    public Vector3 targetScale = Vector3.one;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(
        targetScale.x / transform.lossyScale.x * transform.localScale.x,
        targetScale.y / transform.lossyScale.y * transform.localScale.y,
        targetScale.z / transform.lossyScale.z * transform.localScale.z
        );
    }
}
