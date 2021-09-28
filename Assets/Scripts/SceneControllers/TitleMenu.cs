using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public Button newGameButton, creditsButton, exitButton, backButton, nextButton, startButton;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(newGameButton));
    }

    IEnumerator SelectUI(Button button)
    {
        yield return 1;
        yield return 1;
        EventSystem.current.SetSelectedGameObject(button.gameObject, null);
        button.OnSelect(null);
    }

    public void OnClickCredits()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(backButton));
    }

    public void OnClickNewGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(nextButton));
    }

    public void OnClickNext()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(startButton));
    }

    public void OnClickBack()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SelectUI(newGameButton));
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StartLevel 1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
