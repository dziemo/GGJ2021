using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportController : MonoBehaviour
{
    public StringCollection names;
    public GameObjectCollection lostObjects;
    public List<AreaController> areas = new List<AreaController>();
    public GameEvent onNewReport;

    List<string> gameNames;
    List<GameObject> gameLostObjects;
    List<AreaController> gameAreas;

    AreaController currentArea;
    GameObject currentObject;
    string currentName;

    private void Start()
    {
        gameNames = new List<string>(names);
        gameLostObjects = new List<GameObject>(lostObjects);
        gameAreas = new List<AreaController>(areas);
    }

    public void OnGameStart ()
    {
        GenerateNewReport();
    }

    public void OnLostObjectFound ()
    {
        GenerateNewReport();
    }
    
    private void GenerateNewReport ()
    {
        var newName = gameNames[Random.Range(0, gameNames.Count)];
        var newObject = gameLostObjects[Random.Range(0, gameLostObjects.Count)];
        var newArea = gameAreas[Random.Range(0, gameAreas.Count)];

        var spawnPoint = newArea.PickRandomSpawn();

        gameNames.Remove(currentName);
        gameLostObjects.Remove(currentObject);
        gameAreas.Remove(currentArea);

        Instantiate(newObject, spawnPoint.position, Quaternion.identity);

        if (currentObject)
        {
            gameNames.Add(currentName);
            gameLostObjects.Add(currentObject);
            gameAreas.Add(currentArea);
        }

        currentName = newName;
        currentObject = newObject;
        currentArea = newArea;

        onNewReport.Raise();
    }

}
