using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(3f, 0f, 0f);
    private Vector3 startPos;
    public float speed = 3f;

    private void Start()
    {
        startPos = transform.position;
        transform.DOMove(startPos + moveDirection, speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
