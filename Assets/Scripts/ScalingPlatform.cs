using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingPlatform : MonoBehaviour
{
    public Vector3 minScale = new Vector3(0.5f, 1f, 1f);
    public Vector3 maxScale = new Vector3(1f, 1f, 1f);
    public float speed = 1f;


    public void Awake()
    {
        transform.DOScale(minScale, speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
