using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spring : MonoBehaviour
{
    [SerializeField] Animator SpringAnim;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.player.gameObject == collision.gameObject)
        {
            SpringAnim.SetTrigger("IsTouching");
        }
    }
}