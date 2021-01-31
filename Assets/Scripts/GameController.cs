using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    
    public GameEvent gameStart;
    public GameEvent gameEnd;

    public GameObject endPanel;
    public TextMeshProUGUI endScore;

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
                timeText.text = $"{Mathf.FloorToInt(currentTime / 60f).ToString("0")}:{(currentTime % 60).ToString("00")}";
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
        timeText.text = $"{Mathf.FloorToInt(currentTime / 60f).ToString("0")}:{(currentTime % 60).ToString("00")}";
    }

    private void OnGameEnd ()
    {
        gameEnd.Raise();
        endPanel.SetActive(true);
        endScore.text = score.ToString();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isGameStarted = false;
    }
    
    public void OnGameRestart ()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnGameExit ()
    {
        SceneManager.LoadScene("MainMenu");
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
        timeText.text = $"{Mathf.FloorToInt(currentTime / 60f).ToString("0")}:{(currentTime % 60).ToString("00")}";
        isGameStarted = true;
    }
}
