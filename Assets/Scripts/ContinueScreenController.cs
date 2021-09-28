using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinueScreenController : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private GameManager gameManager;
    private int totalTime;
    private int remainingTime;
    private int startedAt;

    void Start()
    {
        gameManager = GameManager.Instance;
        totalTime = 10;
        remainingTime = totalTime;
        startedAt = (int)Time.timeSinceLevelLoad;
        UpdateTimerText();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameManager.GameState == State.LOSE && gameObject.activeInHierarchy)
        {
            remainingTime = totalTime - ((int)Time.timeSinceLevelLoad - startedAt);
            UpdateTimerText();
            if (remainingTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        startedAt = (int)Time.timeSinceLevelLoad;
        remainingTime = totalTime;
        UpdateTimerText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void UpdateTimerText()
    {
        timerText.text = remainingTime.ToString();
    }

    private void GameOver()
    {
        gameManager.GameOver();
    }
}
