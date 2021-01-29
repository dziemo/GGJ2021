using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public GameEvent gameStart;
    public GameEvent gameEnd;

    public int totalTimeSecs = 60;

    float currentTime;

    bool isGameStarted = false;

    private void Start()
    {
        timeText.enabled = false;   
        StartCoroutine(GameStartDelay());
    }

    private void Update ()
    {
        if (isGameStarted)
        {
            if (currentTime <= 0)
            {
                OnGameEnd();
                timeText.text = 0.00f.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                currentTime -= Time.deltaTime;
                timeText.text = currentTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }

    private void OnGameEnd ()
    {
        Debug.Log("Game end");
        gameEnd.Raise();
        isGameStarted = false;
    }

    IEnumerator GameStartDelay ()
    {
        Debug.Log("Delay start");
        yield return new WaitForSeconds(3f);
        Debug.Log("Game start");
        gameStart.Raise();
        StartCountdown();
    }

    private void StartCountdown ()
    {
        currentTime = totalTimeSecs;
        timeText.enabled = true;
        timeText.text = currentTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        isGameStarted = true;
    }
}
