using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private int startScore = 0;
    private int currentScore;
    void Start()
    {
        currentScore = startScore;
        scoreText.text = "Score : " + currentScore;
    }
    public void AddScore(int score)
    {
        currentScore += score;
        scoreText.text = "Score : " + currentScore;
    }
}