using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private float endTime = 30f;
    [SerializeField] private ResultManager resultManager;

    private bool isGameEnd = false;

    void Update()
    {
        if (isGameEnd) return;

        // 時間を減らす
        endTime -= Time.deltaTime;

        // 小数点なしで表示
        timerText.text = "Time : " + (int)endTime;

        // 0になったら終了
        if (endTime <= 0)
        {
            isGameEnd = true;
            resultManager.ShowResult();
        }
    }
}