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
            StartCoroutine(KillPlayer());
        }    
    }

    public IEnumerator KillPlayer()
    {
        GameManager.Instance.player.KillPlayer();

        yield return new WaitForSeconds(3);

        GameManager.Instance.transition.Close();

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
