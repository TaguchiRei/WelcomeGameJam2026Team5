using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    /// <summary> 残り時間のカウントダウンを表示するUIテキスト </summary>
    [SerializeField] private Text _remainingTimeText;
    /// <summary> ゲーム終了までの残り秒数 </summary>
    [SerializeField] private float _remainingTime = 30f;
    private float _currentTime = 30f;
    [SerializeField] private Image _timerCircle;
    /// <summary> ゲームが終了判定済みかどうかを示すフラグ </summary>
    private bool _isTimeUp = false;
    void Start()
    {
        _currentTime = _remainingTime;
        _timerCircle.fillAmount = 1f;
    }
    void Update()
    {
        _currentTime -= Time.deltaTime;
        _currentTime = Mathf.Max(_currentTime, 0f);
        _timerCircle.fillAmount = _currentTime / _remainingTime;
        if (_isTimeUp) return;

        _remainingTimeText.text = "Time : " + (int)_remainingTime;

        if (_remainingTime <= 0)
        {
            _isTimeUp = true;
            //TODO ここでリザルトを表示するコードを呼び出す
        }
    }
}
