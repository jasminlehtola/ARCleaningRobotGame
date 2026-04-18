using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    // Updates the score and timer text every frame
    void Update()
    {
        if (GameManager.Instance == null) return;

        scoreText.text = "Score: " + GameManager.Instance.score;

        float time = GameManager.Instance.GetTime();
        timerText.text = "Time: " + Mathf.Ceil(time);
    }
}