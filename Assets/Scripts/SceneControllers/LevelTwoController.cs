using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoController : SceneController
{
    public GameObject DialogueContainer;
    public GameObject DialogueOne;
    public GameObject DialogueTwo;
    public GameObject IncomingText;
    public GameObject LevelClearedText;
    public GameObject[] smallAsteroidPrefab;
    public GameObject[] largeAsteroidPrefab;
    public GameObject[] shipPrefab;
    public float asteroidVelocity;
    public GameObject planet;

    private List<GameObject> miniBosses;
    
    void Start()
    {
        base.Start();
        gameManager.GameState = State.UI;
        gameManager.SetHUD();
        StartCoroutine(StartLevelTimeline());
        miniBosses = new List<GameObject>(); 
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
        yield return new WaitForSeconds(7);
        DisplayIncomingMessage();
        yield return new WaitForSeconds(4);
        HideIncomingMessage();
        for (int i = 0; i <= 5; i++)
        {
            while (gameManager.GameState != State.PLAYING)
            {
                yield return new WaitForSeconds(0.5f);
            }
            int delay = LaunchAsteroids(i);
            yield return new WaitForSeconds(2f);
        }
        DisplayIncomingMessage();
        planet.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -0.15f, 0f);
        yield return new WaitForSeconds(4);
        HideIncomingMessage();
        for (int i = 0; i <= 5; i++)
        {
            while (gameManager.GameState != State.PLAYING)
            {
                yield return new WaitForSeconds(0.5f);
            }
            int delay = LaunchShips(i);
            yield return new WaitForSeconds(delay);
        }
        while (!isMiniBossDead())
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2);
        DisplayDialogueTwo();
        yield return new WaitForSeconds(3);
        EndLevel();
    }

    private bool isMiniBossDead()
    {
        foreach (GameObject miniBoss in miniBosses)
        {
            if (miniBoss != null) return false;
        }
        return true;
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

    private int LaunchAsteroids(int index)
    {
        if (index % 5 == 0)
        {
            SpawnAsteroids(5, 3, "top");
            SpawnAsteroids(3, 2, "bottom");
        }
        //else if (index % 10 == 0)
        //{
        //    SpawnAsteroids(5, 3, "top");
        //    SpawnAsteroids(3, 2, "left");
        //    SpawnAsteroids(4, 2, "right");
        //}
        //else if (index % 5 == 0)
        //{
        //    SpawnAsteroids(7, 2, "top");
        //    SpawnAsteroids(3, 0, "right");
        //}
        else if (index % 3 == 0)
        {
            SpawnAsteroids(7, 3, "top");
            SpawnAsteroids(5, 2, "left");
        }
        else if (index % 2 == 0) 
        {
            SpawnAsteroids(7, 2, "top");
            SpawnAsteroids(5, 1, "right");
        }
        else
        {
            SpawnAsteroids(Random.Range(8, 12), 0, "top");
        }
        return 4;
    }

    private void SpawnAsteroids(int small, int large, string location)
    {
        float dist = (transform.position - Camera.main.transform.position).z;
        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        float distanceX = (rightBorder - leftBorder) / (small + large + 1);
        float cumulativeDistanceX = distanceX;
        float distanceY = (topBorder - bottomBorder) / (small + large + 1);
        float cumulativeDistanceY = distanceY;

        for (int i = 0; i < small; i++)
        {
            if (location.Equals("right"))
            {
                float angle = Random.Range(120, 240);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomSmallAsteroid(new Vector3(rightBorder + 0.5f, cumulativeDistanceY, 0f), velocity);
            }
            else if (location.Equals("left"))
            {
                float angle = Random.Range(-60, 60);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomSmallAsteroid(new Vector3(leftBorder - 0.5f, cumulativeDistanceY, 0f), velocity);
            }
            else if (location.Equals("bottom"))
            {
                float angle = Random.Range(30, 150);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomSmallAsteroid(new Vector3(leftBorder + cumulativeDistanceX, topBorder + 0.5f, 0f), velocity);
            }
            else 
            {
                float angle = Random.Range(210, 330);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomSmallAsteroid(new Vector3(leftBorder + cumulativeDistanceX, topBorder + 0.5f, 0f), velocity);
            }

            cumulativeDistanceX += distanceX;
            cumulativeDistanceY += distanceY;
        }

        for (int i = 0; i < large; i++)
        {
            if (location.Equals("right"))
            {
                float angle = Random.Range(120, 240);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomLargeAsteroid(new Vector3(rightBorder + 0.5f, cumulativeDistanceY, 0f), velocity);
            }
            else if (location.Equals("left"))
            {
                float angle = Random.Range(-60, 60);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomLargeAsteroid(new Vector3(leftBorder - 0.5f, cumulativeDistanceY, 0f), velocity);
            }
            else if (location.Equals("bottom"))
            {
                float angle = Random.Range(30, 150);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomLargeAsteroid(new Vector3(cumulativeDistanceX, topBorder + 0.5f, 0f), velocity);
            }
            else
            {
                float angle = Random.Range(210, 330);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 velocity = rotation * new Vector3(asteroidVelocity, 0f, 0f);
                RandomLargeAsteroid(new Vector3(cumulativeDistanceX, topBorder + 0.5f, 0f), velocity);
            }

            cumulativeDistanceX += distanceX;
            cumulativeDistanceY += distanceY;
        }
    }

    private int LaunchShips(int index)
    {
        if (index == 0)
        {
            SpawnShips(Random.Range(3, 5), 0, 0);
        }
        else if (index % 5 == 0)
        {
            SpawnShips(0, 0, 2);
            return 20;
        }
        //else if (index % 10 == 0)
        //{
        //    SpawnShips(0, 2, 1);
        //    return 25;
        //}
        else if (index % 3 == 0)
        {
            SpawnShips(3, 2, 0);
        }
        else
        {
            SpawnShips(Random.Range(3, 5), 0, 0);
        }
        return 5;
    }

    private void SpawnShips(int type1, int type2, int type3)
    {
        float dist = (transform.position - Camera.main.transform.position).z;
        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

        float distanceX = (rightBorder - leftBorder) / (type1 + type2 + type3 + 1);
        float cumulativeDistanceX = distanceX;

        for (int i = 0; i < type1; i++)
        {
            Instantiate(shipPrefab[0], new Vector3(leftBorder + cumulativeDistanceX, Random.Range(5.5f, 7f), 0f), Quaternion.Euler(0f, 0f, 180f));
            cumulativeDistanceX += distanceX;
        }

        for (int i = 0; i < type3; i++)
        {
            miniBosses.Add(Instantiate(shipPrefab[2], new Vector3(leftBorder + cumulativeDistanceX, Random.Range(5.5f, 7f), 0f), Quaternion.identity));
            cumulativeDistanceX += distanceX;
        }

        for (int i = 0; i < type2; i++)
        {
            Instantiate(shipPrefab[1], new Vector3(leftBorder + cumulativeDistanceX, Random.Range(5.5f, 7f), 0f), Quaternion.Euler(0f, 0f, 180f));
            cumulativeDistanceX += distanceX;
        }
    }

    private GameObject RandomSmallAsteroid(Vector3 position, Vector3 velocity)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
        GameObject smallAsteroid = Instantiate(smallAsteroidPrefab[Random.Range(0, smallAsteroidPrefab.Length)], position, rotation);
        smallAsteroid.GetComponent<AsteroidController>().InitialVelocity = velocity;
        return smallAsteroid;
    }

    private GameObject RandomLargeAsteroid(Vector3 position, Vector3 velocity)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
        GameObject largeAsteroid = Instantiate(largeAsteroidPrefab[Random.Range(0, largeAsteroidPrefab.Length)], position, rotation);
        largeAsteroid.GetComponent<AsteroidController>().InitialVelocity = velocity;
        return largeAsteroid;
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
        gameManager.BossBattleTwo();
    }
}
