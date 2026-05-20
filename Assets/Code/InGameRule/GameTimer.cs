using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] private float _endTime = 30f;

    private bool _isGameEnd = false;

    void Update()
    {
        if (_isGameEnd) return;

        // 時間を減らす
        _endTime -= Time.deltaTime;

        // 小数点なしで表示
        _timerText.text = "Time : " + (int)_endTime;

        // 0になったら終了
        if (_endTime <= 0)
        {
            _isGameEnd = true;
            //TODO ここでリザルトを表示するコードを呼び出す
        }
    }
}