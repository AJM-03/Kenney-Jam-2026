using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float verticalThreshold;
    [SerializeField] float moveSpeed;
    [SerializeField] float easeStrength;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance.player.transform.position.y > transform.position.y + verticalThreshold)
        {
            transform.DOKill();
            transform.DOMoveY(GameManager.Instance.player.transform.position.y - verticalThreshold, moveSpeed).SetEase(Ease.OutBack, easeStrength);
        }
    }
}
