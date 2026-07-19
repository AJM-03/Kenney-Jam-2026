using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float verticalThreshold;
    [SerializeField] float moveSpeed;
    [SerializeField] float easeStrength;
    [SerializeField] Transform barrier;

    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
    }

    void Update()
    {
        if (GameManager.Instance.player.transform.position.y > transform.position.y + verticalThreshold)
        {
            transform.DOKill();
            transform.DOMoveY(GameManager.Instance.player.transform.position.y - verticalThreshold, moveSpeed);
        }
    }

    public void BringUpBarrier()
    {
        barrier.DOLocalMove(Vector3.zero, 3);
    }
}
