using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerSegment
{
    public GameObject prefab;
    public int segmentHeight;
}

public class TowerConstruction : MonoBehaviour
{
    [SerializeField] TowerSegment[] towerSegments;
    private GameObject[] loadedSegments;
    [SerializeField] Transform towerParent;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance.player.transform.position.y > loadedSegments[2].transform.position.y)
        {
            LoadSegment();
        }
    }

    public void LoadSegment()
    {
        GameObject newSegment = GameObject.Instantiate(towerSegments[Random.Range(0, towerSegments.Length)].prefab, towerParent);
        newSegment.transform.position = new Vector3(0, loadedSegments[3].transform.position.y, 0);
    }
}
