using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    private Camera cam;

    public Player player;
    public Trajectory trajectory;
    public ScreenTransition transition;


    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(StartTransition());
    }


    public IEnumerator StartTransition()
    {
        transition.ClosePositions();
        yield return new WaitForSeconds(1f);
        transition.Open();
        yield return new WaitForSeconds(3f);
        transition.DisableTransition();
    }
}
