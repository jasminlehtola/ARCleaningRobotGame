using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;

    public float gameDuration = 60f;
    private float timeRemaining;
    public bool gameStarted = false;
    private bool isGameOver = false;

    public GameObject startButton;

    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI finalScoreText;

    void Awake()
    {
        Instance = this;
    }


    // Initializes the game timer
    void Start()
    {
        timeRemaining = gameDuration;
    }

    // Updates the game timer and checks for game over condition
    void Update()
    {
        if (!gameStarted) return;

        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            EndGame();
        }
    }

    // Starts the game, initializing the timer and enabling score collection
    public void StartGame()
    {
        gameStarted = true;
        timeRemaining = gameDuration;

        Debug.Log("Game Started!");

        if (startButton != null)
            startButton.SetActive(false);
    }

    // Adds score to the total score, but only if the game has started or ended
    public void AddScore(int amount)
    {
        if (!gameStarted) return;
        if (isGameOver) return;

        score += amount;
    }

    // Sets the game over flag 
    void EndGame()
    {
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public float GetTime()
    {
        return timeRemaining;
    }
}
