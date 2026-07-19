using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChance : MonoBehaviour
{
    public float spawnChance = 30;
    public int minLayerToStartSpawning = 0;

    private void Awake()
    {
        if (GameManager.Instance.currentScore < minLayerToStartSpawning) gameObject.SetActive(false);
        if (Random.Range(0, 100) > spawnChance) gameObject.SetActive(false);
    }
}
