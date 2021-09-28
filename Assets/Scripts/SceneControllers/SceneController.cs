using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneController : MonoBehaviour
{
    protected GameManager gameManager;
    public ContinueScreenController continueScreen;
    public PlayerController playerController;
    public SoundController soundController;

    public void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameState = State.PLAYING;
    }

    public void Lost()
    {
        gameManager.GameState = State.LOSE;
        continueScreen.Open();
    }

    public void Continue()
    {
        gameManager.GameState = State.PLAYING;
        gameManager.ResetMultiplier();
        gameManager.ResetLife();
        continueScreen.Close();
        playerController.Restart();
    }
}
