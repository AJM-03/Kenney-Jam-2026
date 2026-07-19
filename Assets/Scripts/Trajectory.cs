using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int numberOfDots;
    [SerializeField] GameObject dotParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] Rotate chargeRotate;
    [SerializeField] float chargeRotateSpeed;
    [SerializeField] float dotSpacing;
    [SerializeField][Range(0.01f, 0.3f)] float minDotScale;
    [SerializeField][Range(0.3f, 1f)] float maxDotScale;
    public GameObject previewCursor;
    public Sprite previewOpenSprite;
    public Sprite previewGrabSprite;
    public SpriteRenderer previewRend;
    public float previewSpeed;
    public Vector3 previewClosePosition;

    private Transform[] dots;
    private Vector2 pos;
    private float timeStamp;

    void Start()
    {
        Hide();  // Hide the trajectory
        PrepareDots();
    }

    private void PrepareDots()
    {
        dots = new Transform[numberOfDots];
        dotPrefab.transform.localScale = Vector3.one * maxDotScale;

        float scale = maxDotScale;
        float scaleFactor = scale / numberOfDots;

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, null).transform;
            dots[i].parent = dotParent.transform;

            dots[i].localScale = Vector3.one * scale;
            if (scale > minDotScale) scale -= scaleFactor;
        }
    }

    public void UpdateDots(Vector3 playerPos, Vector2 forceApplied)
    {
        dotParent.transform.position = playerPos;

        timeStamp = dotSpacing;
        for (int i = 0; i < numberOfDots; ++i)
        {
            pos.x = (playerPos.x + forceApplied.x * timeStamp);
            pos.y = (playerPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dots[i].position = pos;
            timeStamp += dotSpacing;
        }
    }

    public void UpdateChargeRotate(float charge, Vector2 force)
    {
        if (force.x > 0) charge *= -1;
        chargeRotate.rotate = new Vector3(0, 0, charge * chargeRotateSpeed);
    }

    public void Show()
    {
        dotParent.SetActive(true);
    }

    public void Hide()
    {
        dotParent.SetActive(false);
    }


    public void StartPreview()
    {
        previewCursor.SetActive(true);
        previewCursor.transform.DOLocalMove(previewClosePosition, previewSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void EndPreview()
    {
        previewCursor.transform.DOKill();
        previewCursor.SetActive(false);
    }
}
