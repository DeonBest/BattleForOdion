using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevelController : SceneController
{
    public GameObject DialogueContainer;
    public GameObject DialogueOne;
    public GameObject DialogueTwo;
    public GameObject IncomingText;
    public GameObject LevelClearedText;
    public GameObject AlienBasicLinear;
    public GameObject AlienBasicCircular;
    private int numWavesDone;
    private bool startWaves;
    public GameObject planet;

    void Start()
    {
        base.Start();
        gameManager.ResetScore();
        gameManager.ResetMultiplier();
        gameManager.SetHUD();
        gameManager.GameState = State.UI;
        StartCoroutine(StartLevelTimeline());
        numWavesDone = 0;
        startWaves = false;
        planet.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -0.55f, 0f);
    }

    IEnumerator StartLevelTimeline()
    {
        yield return new WaitForSeconds(1);
        DisplayDialogue();
        yield return new WaitForSeconds(7);
        DisplayIncomingMessage();
        gameManager.GameState = State.PLAYING;
        yield return new WaitForSeconds(4);
        HideIncomingMessage();
        yield return new WaitForSeconds(2);
        startWaves = true;
        StartAliens();
    }

    IEnumerator EndLevelTimeline()
    {
        yield return new WaitForSeconds(2);
        DisplayDialogueTwo();
        yield return new WaitForSeconds(3);
        EndLevel();
    }

    void Update()
    {
        if (numWavesDone == 5 && CheckAlienShips())
        {
            StartCoroutine(EndLevelTimeline());
            startWaves = false;
            numWavesDone++;
        }

        if (gameManager.GameState == State.PLAYING && startWaves == true)
        {
            if (CheckAlienShips())
            {
                InstanAliens(5);
                numWavesDone++;
            }
        }

        if (gameManager.GameState == State.LOSE && continueScreen.gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Continue();
            }
        }
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

    private void StartAliens()
    {
        Instantiate(AlienBasicLinear, new Vector3(-7f, 6.5f, 0f),  Quaternion.Euler(0f, 0f, 180f));
        Instantiate(AlienBasicCircular, new Vector3(4.5f, 6.5f, 0f), Quaternion.Euler(0f, 0f, 180f));
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


    private void InstanAliens(int num)
    {
        for (int i=0; i<num; i++)
        {
            InstanAlien();
        }
    }

    private void InstanAlien()
    {
        float x = Random.Range(-8f, 5.5f);
        float y = Random.Range(5.5f, 7f);

        int rand = Random.Range(0,100);
        if (rand < 50)
        {
            Instantiate(AlienBasicLinear, new Vector3(x, y, 0f), Quaternion.Euler(0f, 0f, 180f));
        } else
        {
            Instantiate(AlienBasicCircular, new Vector3(x, y, 0f), Quaternion.Euler(0f, 0f, 180f));
        }
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
        gameManager.BossBattleOne();
    }

}
