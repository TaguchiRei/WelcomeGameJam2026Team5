using UnityEngine;

public class PrizeOperation : MonoBehaviour
{
    [SerializeField] private float _forcePower = 5.0f;
    [SerializeField] private string _reflectTag = "Wall";
    [SerializeField] private float _reflectPower = 1.0f;
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private ArrowWriter _arrowWriter;

    private Rigidbody2D _rb;
    private Camera _mainCamera;
    private Vector2 _mouseDownPosition;
    private bool _isDragging;
    private bool _isCaught = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _gameTimer = FindAnyObjectByType<GameTimer>();
    }
    private void Update()
    {
        if (_isDragging)
        {
            Vector2 currentMouse = GetMouseWorldPosition();
            Vector2 force =
                ((Vector2)transform.position - currentMouse) * _forcePower;

            _arrowWriter.Preview(force);
        }
    }

    /// <summary>
    /// マウスが押された位置を記録する
    /// </summary>
    private void OnMouseDown()
    {
        if (_isCaught || _gameTimer.IsTimeUp) return;
        _isDragging = true;
        SetVelocity(Vector2.zero);
    }

    /// <summary>
    /// 押された位置と離した位置の差分を利用して力を加える
    /// </summary>
    private void OnMouseUp()
    {
        if (_isCaught || _gameTimer.IsTimeUp) return;
        _isDragging = false;

        Vector2 mouseUpPosition = GetMouseWorldPosition();
        Vector2 force = (Vector2)transform.position - mouseUpPosition;

        _arrowWriter.Launch(force * _forcePower);
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 掴まれている時に他の景品が当たったら、強制的に落とされる
        if (_isCaught && collision.gameObject.CompareTag("Prize"))
        {
            CraneController crane = GetComponentInParent<CraneController>();
            if (crane != null)
            {
                crane.MoveRelease();
                return;
            }
        }

        if (_isCaught || _isDragging || !collision.gameObject.CompareTag(_reflectTag) || collision.contactCount == 0)
        {
            return;
        }

        Vector2 currentVelocity = GetVelocity();
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflectedVelocity = Vector2.Reflect(currentVelocity, normal);

        SetVelocity(reflectedVelocity * _reflectPower);
    }

    public void BeCaught(Transform parentPoint)
    {
        _isCaught = true;
        _rb.isKinematic = true;
        SetVelocity(Vector2.zero);
        _rb.angularVelocity = 0f;
        transform.SetParent(parentPoint);
        transform.localPosition = Vector3.zero;
    }

    public void BeReleased()
    {
        _isCaught = false;
        transform.SetParent(null);
        _rb.isKinematic = false;
    }

    private Vector2 GetMouseWorldPosition()
    { 
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -_mainCamera.transform.position.z;

        return _mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector2 GetVelocity()
    {
#if UNITY_6000_0_OR_NEWER
        return _rb.linearVelocity;
#else
        return rb.velocity;
#endif
    }

    private void SetVelocity(Vector2 velocity)
    {
#if UNITY_6000_0_OR_NEWER
        _rb.linearVelocity = velocity;
#else
        rb.velocity = velocity;
#endif
    }
}