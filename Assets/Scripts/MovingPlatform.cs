using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(3f, 0f, 0f);
    private Vector3 startPos;
    public float speed = 3f;
    [SerializeField] public LoopType loopType = LoopType.Yoyo;
    [SerializeField] public Ease ease = Ease.InOutSine;
    [SerializeField] public bool local;

    private void Start()
    {
        if (!local)
        {
            startPos = transform.position;
            transform.DOMove(startPos + moveDirection, speed).SetLoops(-1, loopType).SetEase(ease);
        }
        else
        {
            startPos = transform.localPosition;
            transform.DOLocalMove(startPos + moveDirection, speed).SetLoops(-1, loopType).SetEase(ease);
        }
    }
}
