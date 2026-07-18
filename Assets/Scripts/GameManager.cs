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


    private void Start()
    {
        cam = Camera.main;
        player.DeactivateRB();
    }
}
