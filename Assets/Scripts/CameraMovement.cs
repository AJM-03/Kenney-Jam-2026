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

    [SerializeField] Transform cameraTransform;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeAmount = 0.7f;
    [SerializeField] float decreaseFactor = 1.0f;
    [SerializeField] AnimationCurve chargeShake;

    private Vector3 originalPos;
    private float currentShakeDuration;
    private float shakeStrength;


    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        originalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        if (GameManager.Instance.player.transform.position.y > transform.position.y + verticalThreshold)
        {
            transform.DOKill();
            transform.DOMoveY(GameManager.Instance.player.transform.position.y - verticalThreshold, moveSpeed);
            originalPos = cameraTransform.localPosition;
        }


        if (currentShakeDuration > 0)
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * shakeStrength;
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            currentShakeDuration = 0f;
            cameraTransform.localPosition = originalPos;
        }
    }

    public void TriggerDeathShake()
    {
        currentShakeDuration = shakeDuration;
        shakeStrength = 1f;
    }

    public void TriggerChargeShake(float charge)
    {
        currentShakeDuration = 0.1f;
        shakeStrength = chargeShake.Evaluate(charge);
    }

    public void BringUpBarrier()
    {
        barrier.DOLocalMove(Vector3.zero, 3);
    }
}
