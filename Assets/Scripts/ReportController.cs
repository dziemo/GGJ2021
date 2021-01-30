using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ReportController : MonoBehaviour
{
    public StringCollection names;
    public GameObjectCollection lostObjects;
    public List<AreaController> areas = new List<AreaController>();
    public GameEvent onNewReport;
    public TextMeshProUGUI possibleLocationsText;
    public TextMeshProUGUI reportText;

    public AudioSource reportSound;

    public GameObject radio;

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

        var fakeArea = gameAreas[Random.Range(0, gameAreas.Count)];
        var rnd = Random.Range(0f, 2f);

        if (rnd < 1f)
        {
            reportText.text = $"{newName} LOST {newObject.GetComponent<LostObjectController>().objectName}.\nIT MIGHT BE IN {fakeArea.areaName} OR {newArea.areaName}.\nGO FIND IT!";
            possibleLocationsText.text = $"{fakeArea.areaName}\n{newArea.areaName}";
        }
        else
        {
            reportText.text = $"{newName} LOST {newObject.GetComponent<LostObjectController>().objectName}.\nIT MIGHT BE IN {newArea.areaName} OR {fakeArea.areaName}.\nGO FIND IT!";
            possibleLocationsText.text = $"{newArea.areaName}\n{fakeArea.areaName}";
        }

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
        StartCoroutine(ReportAnim());
        reportSound.Play();
    }

    IEnumerator ReportAnim ()
    {
        radio.transform.DOLocalMove(new Vector3(0, 1.5f), 0.5f);
        reportText.transform.parent.GetComponent<RectTransform>().DOAnchorPos3DY(50, 0.5f);
        yield return new WaitForSeconds(5f);
        radio.transform.DOLocalMove(Vector3.zero, 0.5f);
        reportText.transform.parent.GetComponent<RectTransform>().DOAnchorPos3DY(-250, 0.5f);
    }
}
