using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class CraneController : MonoBehaviour
{
    [Header("Crane Horizontal Speed")] //左右移動の移動速度
    [SerializeField] private float _horizontalSpeed = 5f;

    [Header("Crane Vertical Speed")]//アームの移動速度
    [SerializeField] private float _verticalSpeed = 5f;

    [Header("Fall Limit")]//Y座標の下降限界
    [SerializeField] private float _fallLimitY = -1f;

    [Header("Upper Limit")]//Y座標の上昇限界
    [SerializeField] private float _upperLimit = 5f;

    [Header("Start Position")]
    [SerializeField] private Vector2 _startPosition;

    [Header("References")]
    [SerializeField] private Transform _catchPoint; // 景品を固定する場所
    [SerializeField] private ScoreTextManager _scoreTextManager; // お金管理

    private const float _WAITTIME = 1f; // 下がったあとの待機時間 

    private Rigidbody2D _rb2D;

    private bool _isWaiting;

    private CraneStates _currentMode = CraneStates.None;   //開始ModeはNoneでスタート

    private GameObject _caughtPrize = null; // 掴んでいる景品を保持する変数

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        switch (_currentMode)
        {
            case CraneStates.None:
                UpdateMoveHorizontal();
                GetInput();
                break;

            case CraneStates.MoveDown:
                UpdateMoveDown();
                break;

            case CraneStates.MoveUp:
                UpdateMoveUp();
                break;

            case CraneStates.HavePrize:
                UpdateHavePrize();
                break;
        }
    }

    /// <summary>
    /// 左右移動処理
    /// </summary>
    private void UpdateMoveHorizontal()
    {
        float move = Input.GetAxis("Horizontal");
        _rb2D.linearVelocity = new Vector2(move * _horizontalSpeed, 0f);
    }

    /// <summary>
    /// アームDown処理
    /// </summary>
    private void UpdateMoveDown()
    {
        _rb2D.linearVelocity = new Vector2(0f, -_verticalSpeed);
       
        if(_rb2D.position.y <= _fallLimitY)
        {
            if (_isWaiting) return;
            StartCoroutine(MoveCatch());
        }       
    }

    /// <summary>
    /// アームDown状態で1秒間待機
    /// </summary>
    private IEnumerator MoveCatch()
    {
        _isWaiting = true;
        _rb2D.linearVelocity = Vector2.zero; // 待機中は止める

        yield return new WaitForSeconds(_WAITTIME);
        
        // 何か掴んでいれば「景品あり」、空なら「上昇」へ
        if (_caughtPrize != null)
        {
            _currentMode = CraneStates.HavePrize;
        }
        else
        {
            _currentMode = CraneStates.MoveUp;
        }
        _isWaiting = false;
    }

    /// <summary>
    /// アームUp処理（景品がない時）
    /// </summary>
    private void UpdateMoveUp()
    {
        if(_rb2D.position.y < _upperLimit)
        {
            _rb2D.linearVelocity = new Vector2(0f, _verticalSpeed);
        }
        else
        {
            _rb2D.linearVelocity = Vector2.zero;
            _currentMode = CraneStates.None;
        }
    }

    /// <summary>
    /// 景品を取得した状態での上昇・運搬処理
    /// </summary>
    private void UpdateHavePrize()
    {
        if (_rb2D.position.y < _upperLimit)
        {
            _rb2D.linearVelocity = new Vector2(0f, _verticalSpeed);
        }
        else
        {
            // 上まで上がったら、左右操作を受け付ける
            _rb2D.linearVelocity = Vector2.zero;
            ReturnStartPosition();
        }
    }  

    /// <summary>
    /// 景品を運ぶための左右操作
    /// </summary>
    private void ReturnStartPosition()
    {
        float move = Input.GetAxis("Horizontal");
        _rb2D.linearVelocity = new Vector2(move * _horizontalSpeed, 0f);

        // スペースキーで切り離し
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveRelease();
        }
    }

    /// <summary>
    /// アイテムを離す処理
    /// </summary>
    private void MoveRelease()
    {
        if (_caughtPrize != null)
        {
            PrizeOperation prize = _caughtPrize.GetComponent<PrizeOperation>();
            if (prize != null)
            {
                prize.BeReleased();
            }
            _caughtPrize = null;
        }
        RestartState();
    }

    /// <summary>
    /// Spaceキーが押された際の状態変化（お金の支払いも追加）
    /// </summary>
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_scoreTextManager != null) _scoreTextManager.RequestAction(100);
            _currentMode = CraneStates.MoveDown;
        }
    }

    /// <summary>
    /// 開始状態（None）へ戻す
    /// </summary>
    private void RestartState()
    {
        _rb2D.linearVelocity = Vector2.zero;
        _currentMode = CraneStates.None;
    }

    /// <summary>
    /// 景品取得処理
    /// </summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        // 下降中、かつまだ何も掴んでいない時だけキャッチ
        if (_currentMode == CraneStates.MoveDown && _caughtPrize == null)
        {
            if (other.gameObject.CompareTag("Prize"))
            {
                PrizeOperation prize = other.GetComponent<PrizeOperation>();
                if (prize != null)
                {
                    _caughtPrize = other.gameObject;
                    prize.BeCaught(_catchPoint);
                }
            }
        }
    }
}
