using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.Instance.player.gameObject == collision.gameObject)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }    
    }
}
