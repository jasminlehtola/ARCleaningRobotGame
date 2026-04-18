using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;

    public float gameDuration = 90f;
    private float timeRemaining;
    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        timeRemaining = gameDuration;
    }

    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            EndGame();
        }
    }


    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
    }

    void EndGame()
    {
        isGameOver = true;
        Debug.Log("GAME OVER! Final score: " + score);
    }

    public float GetTime()
    {
        return timeRemaining;
    }
}
