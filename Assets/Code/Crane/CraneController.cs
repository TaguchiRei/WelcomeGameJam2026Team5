using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CraneController : MonoBehaviour
{
    [Header("Control")] [SerializeField] private KeyCode _leftKey = KeyCode.A;
    [SerializeField] private KeyCode _rightKey = KeyCode.D;
    [SerializeField] private KeyCode _downKey = KeyCode.Space;

    [Header("Crane Horizontal Speed")] //左右移動の移動速度
    [SerializeField]
    private float _horizontalSpeed = 5f;

    [Header("Crane Vertical Speed")] //アームの移動速度
    [SerializeField]
    private float _verticalSpeed = 5f;

    [Header("Fall Limit")] //Y座標の下降限界
    [SerializeField]
    private float _fallLimitY = -1f;

    [Header("Upper Limit")] //Y座標の上昇限界
    [SerializeField]
    private float _upperLimit = 5f;

    [Header("Start Position")] [SerializeField]
    private Vector2 _startPosition;

    [Header("References")] [SerializeField]
    private Transform _catchPoint; // 景品を固定する場所

    [SerializeField] private ScoreTextManager _scoreTextManager; // お金管理

    private const float _WAITTIME = 1f; // 下がったあとの待機時間 

    private Rigidbody2D _rb2D;

    private bool _isWaiting;

    private CraneState _currentMode = CraneState.None; //開始ModeはNoneでスタート

    private PrizeOperation _caughtPrize; // 掴んでいる景品を保持する変数

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (_currentMode)
        {
            case CraneState.None:
                UpdateMoveHorizontal();
                GetInput();
                break;

            case CraneState.MoveDown:
                UpdateMoveDown();
                break;

            case CraneState.MoveUp:
                UpdateMoveUp();
                break;

            case CraneState.HavePrize:
                UpdateHavePrize();
                break;
        }
    }

    /// <summary>
    /// 左右移動処理
    /// </summary>
    private void UpdateMoveHorizontal()
    {
        float move = GetAxisHorizontal();
        _rb2D.linearVelocity = new Vector2(move * _horizontalSpeed, 0f);
    }

    /// <summary>
    /// アームDown処理
    /// </summary>
    private void UpdateMoveDown()
    {
        _rb2D.linearVelocity = new Vector2(0f, -_verticalSpeed);

        if (_rb2D.position.y <= _fallLimitY)
        {
            if (_isWaiting) return;
            _currentMode = CraneState.MoveUp;
        }
    }

    /// <summary>
    /// アームUp処理（景品がない時）
    /// </summary>
    private void UpdateMoveUp()
    {
        if (_rb2D.position.y < _upperLimit)
        {
            _rb2D.linearVelocity = new Vector2(0f, _verticalSpeed);
        }
        else
        {
            _rb2D.linearVelocity = Vector2.zero;
            _currentMode = CraneState.None;
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
            UpdateMoveHorizontalHavePrize();
        }
    }

    /// <summary>
    /// 景品を運ぶための左右操作
    /// </summary>
    private void UpdateMoveHorizontalHavePrize()
    {
        float move = GetAxisHorizontal();
        _rb2D.linearVelocity = new Vector2(move * _horizontalSpeed, 0f);

        // 下がる時と同じキーで落とす
        if (Input.GetKeyDown(_downKey))
        {
            MoveRelease();
        }
    }

    /// <summary>
    /// アイテムを離す処理
    /// </summary>
    public void MoveRelease()
    {
        if (_caughtPrize != null)
        {
            _caughtPrize.BeReleased();
            _caughtPrize = null;
        }

        // 途中で落としても必ず上まで戻るようにする
        _currentMode = CraneState.MoveUp;
    }

    /// <summary>
    /// Spaceキーが押された際の状態変化（お金の支払いも追加）
    /// </summary>
    private void GetInput()
    {
        if (Input.GetKeyDown(_downKey))
        {
            if (_scoreTextManager != null) _scoreTextManager.OnCustomerAction(100);
            _currentMode = CraneState.MoveDown;
        }
    }

    /// <summary>
    /// 開始状態へ戻す
    /// </summary>
    private void RestartState()
    {
        _rb2D.linearVelocity = Vector2.zero;
        _currentMode = CraneState.None;
    }

    private int GetAxisHorizontal()
    {
        int i = 0;
        if (Input.GetKey(_leftKey))
        {
            i--;
        }

        if (Input.GetKey(_rightKey))
        {
            i++;
        }

        return i;
    }

    /// <summary>
    /// 景品取得処理
    /// </summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        // 下降中、かつまだ何も掴んでいない時だけキャッチ
        if (_currentMode == CraneState.MoveDown && _caughtPrize == null)
        {
            if (other.gameObject.CompareTag("Prize"))
            {
                PrizeOperation prize = other.GetComponent<PrizeOperation>();
                if (prize != null)
                {
                    _caughtPrize = prize;
                    prize.BeCaught(_catchPoint);
                    _currentMode = CraneState.HavePrize;
                }
            }
        }
    }
}