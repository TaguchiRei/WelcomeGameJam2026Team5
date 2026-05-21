using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public bool IsTimeUp => _currentTime <= 0;
    /// <summary> 残り時間のカウントダウンを表示するUIテキスト </summary>
    [SerializeField] private Text _remainingTimeText;
    /// <summary> ゲーム終了までの残り秒数 </summary>
    [SerializeField] private float _remainingTime = 30f; 

    /// <summary> ゲームが終了判定済みかどうかを示すフラグ </summary>
    private bool _isTimeUp;

    private float _currentTime = 0;

    private void Start()
    {
        _currentTime = _remainingTime;
    }

    void Update()
    {
        if (_isTimeUp) return;

        _currentTime -= Time.deltaTime;

        _remainingTimeText.text = "Time : " + (int)_currentTime ;

        if (_currentTime <= 0)
        {
            _isTimeUp = true;
            //TODO ここでリザルトを表示するコードを呼び出す
        }
    }
}
