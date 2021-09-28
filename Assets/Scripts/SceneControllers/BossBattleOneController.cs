using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleOneController : SceneController
{
    public GameObject DialogueContainer;
    public GameObject DialogueOne;
    public GameObject DialogueTwo;
    public GameObject IncomingText;
    public GameObject LevelClearedText;
    public BackgroundScroll levelBackground;
    public GameObject boss;

    void Start()
    {
        base.Start();
        gameManager.GameState = State.UI;
        gameManager.SetHUD();
        StartCoroutine(StartLevelTimeline());
}

    void Update()
    {
        if (gameManager.GameState == State.LOSE && continueScreen.gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Continue();
            }
        }
    }

    IEnumerator StartLevelTimeline()
    {
        yield return new WaitForSeconds(1);
        DisplayDialogue();
        yield return new WaitForSeconds(3);
        DisplayIncomingMessage();
        gameManager.GameState = State.PLAYING;
        yield return new WaitForSeconds(4);
        HideIncomingMessage();
        levelBackground.StopScrolling();
        while (!isBossDead())
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2);
        DisplayDialogueTwo();
        yield return new WaitForSeconds(3);
        EndLevel();
    }

    private bool isBossDead()
    {
        return boss == null;
    }

    private void DisplayDialogue()
    {
        DialogueContainer.SetActive(true);
        DialogueOne.SetActive(true);
    }

    private void DisplayDialogueTwo()
    {
        DialogueContainer.SetActive(true);
        DialogueTwo.SetActive(true);
    }

    private void DisplayIncomingMessage()
    {
        DialogueContainer.SetActive(false);
        DialogueOne.SetActive(false);
        IncomingText.SetActive(true);
    }

    private void HideIncomingMessage()
    {
        IncomingText.SetActive(false);
    }

    private bool CheckAlienShips()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("AlienShip");
        if (allObjects.Length > 0)
        {
            return false;
        }
        return true;
    }

    private void EndLevel()
    {
        gameManager.GameState = State.UI;
        GetComponent<AudioSource>().Stop();
        soundController.PlayAudio("win");
        playerController.WinMovement();
        LevelClearedText.SetActive(true);
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(6);
        gameManager.Level2();
    }
}
