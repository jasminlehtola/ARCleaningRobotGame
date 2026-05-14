using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public static GameManager Instance;
    public int score = 0;
    public float gameDuration = 60f;

    private bool planesDetected = false;
    private float timeRemaining;
    public bool gameStarted = false;
    private bool isGameOver = false;

    public GameObject startButton;
    public GameObject gameOverPanel;
    public GameObject instructions;
    public GameObject startScreen;
    public GameObject loadingPanel;
    public TMPro.TextMeshProUGUI finalScoreText;

    public AudioSource vacuumSource;

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
        if (!planesDetected && planeManager.trackables.count > 0)
        {
            planesDetected = true;

            loadingPanel.SetActive(false);
            startScreen.SetActive(true);
        }

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
        vacuumSource.Play();

        startScreen.SetActive(false);
        Debug.Log("Game Started!");

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
        vacuumSource.Stop();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;
    }

    // Restarts the game by reloading the current scene
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
