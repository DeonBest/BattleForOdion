using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button backButton;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            scoreText.SetText("Final Score: " + gameManager.GetScore());
        }
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(backButton));
    }

    IEnumerator SelectUI(Button button)
    {
        yield return 1;
        yield return 1;
        EventSystem.current.SetSelectedGameObject(button.gameObject, null);
        button.OnSelect(null);
    }

    public void RestartGame()
    {
        if (gameManager != null) 
        {
            gameManager.Restart();
        }
        SceneManager.LoadScene("TitleScene");
    }
}
