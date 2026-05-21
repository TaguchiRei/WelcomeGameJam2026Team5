using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class winner : MonoBehaviour
{
    public int teamA_Score = 0;
    public int teamB_Score = 0;

    public Text resultText;

    private Color flashColor = new Color32(24, 27, 57, 255);
    private Color drawColor = new Color32(255, 233, 170, 255);

    private Color originalColor;

    void Start()
    {
        originalColor = resultText.color;
    }
    public void SetScore(int a, int b) 
    {
        teamA_Score = a;
        teamB_Score = b;
    }

    public string GetResult() 
    {
        if (teamA_Score > teamB_Score)
        {
            return "Team A Wins";
        }
        else if (teamA_Score < teamB_Score)
        {
            return "Team B Wins";
        }
        else 
        {
            return "Draw";
        }
    }

    public void ShowResult()
    {
        string result = GetResult();

        resultText.text = result;
        
        Debug.Log(GetResult());

        if (result != "Draw")
            {
                StartCoroutine(FlashText(drawColor));
            }
        else
            {
                StartCoroutine(FlashText(flashColor));
            }
    }

    IEnumerator FlashText(Color targetColor)
    {
        while (true)
        {
            resultText.color = targetColor;

            yield return new WaitForSeconds(0.3f);

            resultText.color = originalColor;

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
