using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TowerConstruction : MonoBehaviour
{
    [SerializeField] TowerSegment[] towerSegments;
    private GameObject[] loadedSegments = new GameObject[4];
    [SerializeField] Transform towerParent;

    void Start()
    {
        for (int i = 0; i < loadedSegments.Length; i++)
        {
            LoadSegment();
        }
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
        GameObject highestSegment = null;

        for (int i = 0; i < loadedSegments.Length; i++)
            if (loadedSegments[i] != null) highestSegment = loadedSegments[i];
        if (highestSegment != null) newSegment.transform.position = new Vector3(0, highestSegment.transform.position.y + (highestSegment.GetComponent<TowerPart>().segment.segmentHeight / 2.0f) + (newSegment.GetComponent<TowerPart>().segment.segmentHeight / 2.0f), 0);
        
        if (loadedSegments[0]) Destroy(loadedSegments[0]);
        loadedSegments[0] = loadedSegments[1];
        loadedSegments[1] = loadedSegments[2];
        loadedSegments[2] = loadedSegments[3];
        loadedSegments[3] = newSegment;
    }
}
