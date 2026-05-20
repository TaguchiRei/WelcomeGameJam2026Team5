using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private int startScore = 0;
    private int endScore;
    void Start()
    {
        endScore= startScore;
        scoreText.text = "Score : " + endScore;
    }
    public void AddScore(int score)
    {
        endScore += score;
        scoreText.text = "Score :" + endScore;
    }
}