using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    public string areaName;
    public Transform spawnsParent;

    List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        foreach (Transform s in spawnsParent)
        {
            spawnPoints.Add(s);
        }
    }

    public Transform PickRandomSpawn ()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
