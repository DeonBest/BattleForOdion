using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public GameObject[] livesDisplay;

    public void UpdateScore(long score)
    {
        scoreText.SetText(string.Format("{0:n0}", score));
    }

    public void UpdateMultiplier(float multiplier)
    {
        multiplierText.SetText("x " + string.Format("{0:n}", multiplier));
    }

    public void UpdateLives(int lives)
    {
        for (int i = 0; i < livesDisplay.Length; i++)
        {
            if (i + 1 <= lives)
            {
                livesDisplay[i].SetActive(true);
            }
            else
            {
                livesDisplay[i].SetActive(false);
            }
        }
    }
}
