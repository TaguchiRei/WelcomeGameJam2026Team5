using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text scoreText;
    public Text prizeGetText;

    private int totalScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void OnPrizeGet(PrizeData prize)
    {
        totalScore += prize.price;

        scoreText.text = $"Score: {totalScore}";
        prizeGetText.text = $"{prize.prizeName} ゲット！ +{prize.price} 点";

        StartCoroutine(FadeOutPrizeText());
    }

    private System.Collections.IEnumerator FadeOutPrizeText()
    {
        Color c = prizeGetText.color;
        c.a = 1f;
        prizeGetText.color = c;

        yield return new WaitForSeconds(1f);

        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            c.a = 1f - t;
            prizeGetText.color = c;
            yield return null;
        }
    }
}
