using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerSegment", menuName = "Tower")]
public class TowerSegment : ScriptableObject
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int segmentHeight;
}