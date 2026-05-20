using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class CraneController : MonoBehaviour
{
    [Header("Crane Horizontal Speed")] //水平方向の移動速度
    [SerializeField] private float _horizontalSpeed = 5f;

    [Header("Crane Vertical Speed")]//垂直方向の移動速度
    [SerializeField] private float _verticalSpeed = 5f;

    [Header("Fall Limit")]//Y座標の落下限界
    [SerializeField] private float _fallLimitY = 5f;

    [Header("Upper Limit")]//Y座標の上昇限界
    [SerializeField] private float _upperLimit = 5f;

    [Header("Start Position")]
    [SerializeField] private Vector2 _startPosition;

    //キー指定
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;
    [SerializeField] private KeyCode _downKey;

    private Rigidbody2D _rb2D;

    private CraneStates nowMode = CraneStates.None;   //初期ModeはNoneでスタート

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        switch (nowMode)
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
        int move = 0;

        if (Input.GetKey(_leftKey))
        {
            move = -1;
        }
        else if (Input.GetKey(_rightKey))
        {
            move = 1;
        }

        _rb2D.linearVelocity = new Vector3(move * _horizontalSpeed, 0f, 0f);
    }

    /// <summary>
    /// アームDown処理
    /// </summary>
    private void UpdateMoveDown()
    {
        _rb2D.linearVelocity = new Vector3(0f, -_verticalSpeed, 0f);
       
        if(_rb2D.position.y <= _fallLimitY)
        {
            nowMode = CraneStates.MoveUp;
            return;
        }       
    }

    /// <summary>
    /// アームUp処理 + prize未取得の時はNoneに状態移行処理
    /// </summary>
    private void UpdateMoveUp()
    {
        if(_rb2D.position.y < _upperLimit)
        {
            _rb2D.linearVelocity = new Vector3(0f, _verticalSpeed, 0f);
        }

        else
        {
            nowMode = CraneStates.None;
            return;
        }
    }

    /// <summary>
    /// アームUp処理　＋ prize取得のときstartPositionに移動するメソッドを取得
    /// </summary>
    private void UpdateHavePrize()
    {
        _rb2D.linearVelocity = new Vector3(0f, _verticalSpeed, 0f);

        if (_rb2D.position.y < _upperLimit)
        {
            ReturnStartPosition();
        }
    }  

    /// <summary>
    /// x座標をstartPositionの位置に戻す処理
    /// </summary>
    private void ReturnStartPosition()
    {
        if(_rb2D.position.x > _startPosition.x)
        {
            _rb2D.linearVelocity = new Vector3(_horizontalSpeed, 0f, 0f);
        }

        if(_rb2D.position.x <=  _startPosition.x)
        {
            MoveRelease();
        } 
    }

    /// <summary>
    /// アイテムを離す処理
    /// </summary>
    private void MoveRelease()
    {
        ResetState();
    }

    /// <summary>
    /// Spaceキーが押された際に状態変化処理
    /// </summary>
    private void GetInput()
    {
        if (Input.GetKeyDown(_downKey))
        {
            nowMode = CraneStates.MoveDown;
            return;
        }
    }

    /// <summary>
    ///　アームがStartPositionにいった後の状態変化処理
    /// </summary>
    private void ResetState()
    {
        nowMode = CraneStates.None;
        return;
    }

    /// <summary>
    /// prize取得処理
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Prize"))
        {
            nowMode = CraneStates.MoveUp;
            return;
        }
    }
}
