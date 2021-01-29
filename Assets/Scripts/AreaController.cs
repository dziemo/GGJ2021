using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    List<Transform> spawnPoints = new List<Transform>();

    public Transform spawnsParent;

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
