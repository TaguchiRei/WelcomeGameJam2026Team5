using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Winner : MonoBehaviour
{
    public int teamA_Score = 0;
    public int teamB_Score = 0;

    public TextMeshProUGUI resultText;

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
            return "客側の勝利！";
        }
        else if (teamA_Score < teamB_Score)
        {
            return "店側の勝利！";
        }
        else
        {
            return "引き分け";
        }
    }

    public void ShowResult()
    {
        gameObject.SetActive(true);
        string result = GetResult();

        resultText.text = result;

        Debug.Log(GetResult());

        if (result != "引き分け")
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
}
