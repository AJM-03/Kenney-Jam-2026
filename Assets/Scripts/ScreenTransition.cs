using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public Transform left;
    public Vector3 leftOpenPos;
    public Vector3 leftClosePos;

    public Transform right;
    public Vector3 rightOpenPos;
    public Vector3 rightClosePos;

    public float openSpeed;
    public Ease openEase;

    public float closeSpeed;
    public Ease closeEase;


    public void ClosePositions()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        left.transform.position = leftClosePos;
        right.transform.position = rightClosePos;
    }

    public void OpenPositions()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        left.transform.position = leftOpenPos;
        right.transform.position = rightOpenPos;
    }

    public void DisableTransition()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
    }

    public void Close()
    {
        OpenPositions();
        left.DOLocalMove(leftClosePos, closeSpeed).SetEase(closeEase);
        right.DOLocalMove(rightClosePos, closeSpeed).SetEase(closeEase);
    }


    public void Open()
    {
        ClosePositions();
        left.DOLocalMove(leftOpenPos, openSpeed).SetEase(openEase);
        right.DOLocalMove(rightOpenPos, openSpeed).SetEase(openEase);
    }
}
