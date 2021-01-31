using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    
    public GameEvent gameStart;
    public GameEvent gameEnd;

    public int totalTimeSecs = 60;
    public int score = 0;
    public float collisionPenalty = 1f;
    float currentTime;

    bool isGameStarted = false;

    private void Start()
    {
        timeText.enabled = false;
        scoreText.text = score.ToString();
        StartCoroutine(GameStartDelay());
    }

    private void Update ()
    {
        if (isGameStarted)
        {
            if (currentTime <= 0)
            {
                OnGameEnd();
                timeText.text = 0f.ToString("0");
                timeText.text = "0:00";
            }
            else
            {
                currentTime -= Time.deltaTime;
                timeText.text = currentTime.ToString("0");
                timeText.text = $"{((currentTime / 60) - 1).ToString("0")}:{(currentTime % 60).ToString("0")}";
            }
        }
    }

    public void OnLostObjectFound ()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void OnCustomerCollision()
    {
        currentTime -= collisionPenalty;
        timeText.text = $"{((currentTime / 60) - 1).ToString("0")}:{(currentTime % 60).ToString("0")}";
    }

    private void OnGameEnd ()
    {
        gameEnd.Raise();
        isGameStarted = false;
    }
    
    IEnumerator GameStartDelay ()
    {
        yield return new WaitForSeconds(3f);
        gameStart.Raise();
        StartCountdown();
    }

    private void StartCountdown ()
    {
        currentTime = totalTimeSecs;
        timeText.enabled = true;
        timeText.text = $"{((currentTime / 60) - 1).ToString("0")}:{(currentTime % 60).ToString("0")}";
        isGameStarted = true;
    }
}
