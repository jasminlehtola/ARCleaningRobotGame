using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;

    public float gameDuration = 90f;
    private float timeRemaining;
    public bool gameStarted = false;
    private bool isGameOver = false;

    public GameObject startButton;

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
    }

    public float GetTime()
    {
        return timeRemaining;
    }
}
