using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    /// <summary> 残り時間のカウントダウンを表示するUIテキスト </summary>
    [SerializeField] private Text _remainingTimeText;
    /// <summary> ゲーム終了までの残り秒数 </summary>
    [SerializeField] private float _remainingTime = 30f;

    /// <summary> ゲームが終了判定済みかどうかを示すフラグ </summary>
    private bool _isTimeUp;
    public bool IsTimeUp => IsTimeUp;

    void Update()
    {
        if (_isTimeUp) return;

        _remainingTime -= Time.deltaTime;

        _remainingTimeText.text = "Time : " + (int)_remainingTime;

        if (_remainingTime <= 0)
        {
            _isTimeUp = true;
            //TODO ここでリザルトを表示するコードを呼び出す
        }
    }
}
