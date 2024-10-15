using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;
    public int score { get; private set; } = 0;

    public float levelTime = 400f; // Time for the level in seconds
    private float currentTime;

    public HUDManager hudManager; // Reference to HUDManager

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }

    public void NewGame()
    {
        lives = 3;
        coins = 0;
        score = 0;
        currentTime = levelTime;

        UpdateHUD();
        LoadLevel(1, 1);
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f); // Start the timer
    }

    public void GameOver()
    {
        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
        currentTime = levelTime; // Reset timer for the new level
        UpdateHUD();
    }

    public void NextLevel()
    {
        // Calculate bonus score based on remaining time
        int bonusScore = Mathf.RoundToInt(currentTime * 10); // Adjust multiplier as needed

        // Add bonus score to total score
        score += bonusScore;

        MarioAgent rewardAgent = FindObjectOfType<MarioAgent>();
        if (rewardAgent != null)
        {
            rewardAgent.AddReward(bonusScore / 1000f); // Adjust reward scaling as needed
        }

        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;
        UpdateHUD(); // Update HUD after losing a life

        // Penalize the agent for losing a life
        MarioAgent rewardAgent = FindObjectOfType<MarioAgent>();
        if (rewardAgent != null)
        {
            rewardAgent.AddReward(-1.0f); // Adjust penalty value as needed
        }

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;
        UpdateHUD(); // Update HUD when collecting a coin

        // Reward the agent for collecting a coin
        MarioAgent rewardAgent = FindObjectOfType<MarioAgent>();
        if (rewardAgent != null)
        {
            rewardAgent.AddReward(1.0f); // Adjust reward value as needed
        }


        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
        UpdateHUD(); // Update HUD when gaining a life
    }

    public void AddScore(int amount)
    {
        score += amount;
        // Update HUD with new score
        if (hudManager != null)
        {
            hudManager.UpdateScore(score);
        }
    }

    public void OnGoombaKilled()
    {
        AddScore(100); // Adjust the point value as needed
    }

    public void OnMagicMushroomCollected()
    {
        AddScore(500); // Adjust the point value as needed
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime--;

            if (hudManager != null)
            {
                hudManager.UpdateTime((int)currentTime); // Update the HUD timer display
            }
        }
        else
        {
            ResetLevel();
        }
    }

    private void UpdateHUD()
    {
        if (hudManager != null)
        {
            hudManager.UpdateWorld($"{world}-{stage}");
            hudManager.UpdateLives(lives);
            hudManager.UpdateCoins(coins);
            hudManager.UpdateScore(score);
            hudManager.UpdateTime((int)currentTime); // Update the timer display in the HUD
        }
    }
}
