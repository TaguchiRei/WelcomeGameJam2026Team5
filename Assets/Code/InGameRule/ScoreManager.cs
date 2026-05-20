using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private int _startScore = 0;
    private int _currentScore;
    void Start()
    {
        _currentScore = _startScore;
        _scoreText.text = "Score : " + _currentScore;
    }
    public void AddScore(int score)
    {
        _currentScore += score;
        _scoreText.text = "Score : " + _currentScore;
    }
}