using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TMP_Text scoreText; // Optional, if you are using a score system
    public TMP_Text coinsText;
    public TMP_Text worldText;
    public TMP_Text timeText;
    public TMP_Text livesText;

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE\n" + score.ToString();
            Debug.Log("Updated score text: " + scoreText.text); // For debugging
        }
        else
         {
           Debug.Log("scoreText is null");
         }
    }

    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = "COINS\n" + coins.ToString();
            Debug.Log("Updated coins text: " + coinsText.text); // For debugging
        }
        else
    {
        Debug.Log("coinText is null");
    }
    }

    public void UpdateWorld(string world)
    {
        if (worldText != null)
        {
            worldText.text = "WORLD\n" + world;
            Debug.Log("Updated world text: " + worldText.text); // For debugging
        }
        else
    {
        Debug.Log("worldText is null");
    }
    }

    public void UpdateTime(int time)
    {
        if (timeText != null)
        {
            timeText.text = "TIME\n" + time.ToString();
            Debug.Log("Updated Time text: " + timeText.text); // For debugging
        }
        else
    {
        Debug.Log("timeText is null");
    }
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "LIVES\n" + lives.ToString();
            Debug.Log("Updated Lives text: " + livesText.text); // For debugging
        }
        else
    {
        Debug.Log("livesText is null");
    }
    }
}
