using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTwoController : SceneController
{
    public GameObject DialogueContainer;
    public GameObject DialogueOne;
    public GameObject DialogueTwo;
    public GameObject IncomingText;
    public GameObject LevelClearedText;
    public GameObject planet;
    public BackgroundScroll levelBackground;
    public GameObject boss;

    void Start()
    {
        base.Start();
        gameManager.GameState = State.UI;
        gameManager.SetHUD();
        StartCoroutine(StartLevelTimeline());
        planet.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -0.15f, 0f);
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
        yield return new WaitForSeconds(4);
        HideIncomingMessage();
        planet.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
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
        gameManager.GameState = State.PLAYING;
    }

    private void HideIncomingMessage()
    {
        IncomingText.SetActive(false);
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
        gameManager.Victory();
    }
}
