using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    UI,
    PLAYING,
    WIN,
    LOSE
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public State GameState { get; set; } = State.UI;

    public int MAX_LIVES;

    private HUDController hudController;
    private long score;
    private float multiplier;
    private int lives;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.hudController = null;
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        score = 0;
        multiplier = 1;
        lives = MAX_LIVES;
    }

    public void SetHUD()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud != null)
        {
            hudController = hud.GetComponent<HUDController>();
            SetScoreText();
            SetMultiplierText();
            SetLivesDisplay();
        }
    }
    public string GetScore()
    {
        return string.Format("{0:n0}", score);
    }

    public int GetLife()
    {
        return lives;
    }

    private void SetScoreText()
    {
        hudController.UpdateScore(score);
    }

    private void SetMultiplierText()
    {
        hudController.UpdateMultiplier(multiplier);
    }

    public void SetLivesDisplay()
    {
        hudController.UpdateLives(lives);
    }

    public void ResetScore()
    {
        score = 0;
        if (hudController != null) SetScoreText();
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        if (hudController != null) SetMultiplierText();
    }

    public void ResetLife()
    {
        lives = MAX_LIVES;
        if (hudController != null) SetLivesDisplay();
    }

    public void Restart()
    {
        ResetScore();
        ResetMultiplier();
        ResetLife();
    }

    public void BossBattleTwo()
    {
        SceneManager.LoadScene("BossBattle2");
    }

    public void BossBattleOne()
    {
        SceneManager.LoadScene("BossBattle1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Victory()
    {
        SceneManager.LoadScene("Victory");
    }

    public void UpdateScore(long points)
    {
        score += (long)(multiplier * points);
        SetScoreText();
        UpdateMultiplier();
    }

    public void UpdateMultiplier()
    {
        multiplier += 0.1f;
        SetMultiplierText();
    }

    public void DecreaseLife()
    {
        lives--;
        SetLivesDisplay();
    }
}
