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
                timeText.text = 0.00f.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                currentTime -= Time.deltaTime;
                timeText.text = currentTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
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
        Debug.Log("TIME PENALTY : " + collisionPenalty);
        currentTime -= collisionPenalty;
        timeText.text = currentTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
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
        timeText.text = currentTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        isGameStarted = true;
    }
}
