using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextManager : MonoBehaviour
{
    [SerializeField] private Text _scoreTextA;
    [SerializeField] private Text _scoreTextB;

    private Coroutine _blinkA;
    private Coroutine _blinkB;
    private Color _baseColor = new(1f, 0.84f, 0.4f);
    
    private int _scoreA;
    private int _scoreB;
    private int _totalSpentMoney;

    public void RequestAction(int cost)
    {
        _totalSpentMoney += cost;
        _scoreTextB.text = "SPENT: ¥" + _totalSpentMoney.ToString();
    }

    public void AddScoreA(int value)
    {
        _scoreA += value;
        _scoreTextA.text = _scoreA.ToString();

        if (_blinkA != null) StopCoroutine(_blinkA);
        _blinkA = StartCoroutine(Blink(_scoreTextA, Color.blue));
    }

    public void AddScoreB(int value)
    {
        _scoreB += value;
        _scoreTextB.text = _scoreB.ToString();

        if (_blinkB != null) StopCoroutine(_blinkB);
        _blinkB = StartCoroutine(Blink(_scoreTextB, Color.red));
    }

    IEnumerator Blink(Text t, Color flashColor)
    {
        Color originalColor = _baseColor;

        for (int i = 0; i < 3; i++)
        {
            t.color = flashColor;
            yield return new WaitForSeconds(0.1f);

            t.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    
    void Start()
    {
        _scoreTextA.color = _baseColor;
        _scoreTextB.color = _baseColor;
    }
}