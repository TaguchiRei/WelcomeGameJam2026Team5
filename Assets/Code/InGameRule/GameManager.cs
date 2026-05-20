using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Text ScoreTextA;
    public Text ScoreTextB;

    private int scoreA;
    private int scoreB;

    private Coroutine blinkA;
    private Coroutine blinkB;

    private Color baseColor = new Color(1f, 0.84f, 0.4f);

    public void AddScoreA(int value)
    {
        scoreA += value;
        ScoreTextA.text = scoreA.ToString();

        if (blinkA != null) StopCoroutine(blinkA);
        blinkA = StartCoroutine(Blink(ScoreTextA, Color.blue));
    }

    public void AddScoreB(int value) 
    {
        scoreB += value;
        ScoreTextB.text = scoreB.ToString();

        if (blinkB != null) StopCoroutine(blinkB);
        blinkB = StartCoroutine(Blink(ScoreTextB, Color.red));
    }

    IEnumerator Blink(Text t, Color flashColor)
    {
        Color originalColor = baseColor;
        
        for (int i = 0; i < 3; i++) 
        {
            t.color = flashColor;
            yield return new WaitForSeconds(0.1f);

            t.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreTextA.color = baseColor;
        ScoreTextB.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
