using UnityEngine;

public class PrizeOperation : MonoBehaviour
{
    [SerializeField] private float forcePower = 5.0f;
    [SerializeField] private string reflectTag = "Wall";
    [SerializeField] private float reflectPower = 1.0f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 mouseDownPosition;
    private bool isDragging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// マウスが押された位置を記録する
    /// </summary>
    private void OnMouseDown()
    {
        isDragging = true;
        mouseDownPosition = GetMouseWorldPosition();
        SetVelocity(Vector2.zero);
    }

    /// <summary>
    /// マウスを押している間は移動可能にする
    /// </summary>
    private void OnMouseDrag()
    {
        rb.MovePosition(GetMouseWorldPosition());
    }

    /// <summary>
    /// 押された位置と離した位置の差分を利用して力を加える
    /// </summary>
    private void OnMouseUp()
    {
        isDragging = false;

        Vector2 mouseUpPosition = GetMouseWorldPosition();
        Vector2 force = mouseDownPosition - mouseUpPosition;

        rb.AddForce(force * forcePower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 壁などに当たったとき、衝突面の向きに合わせて反射する
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDragging || !collision.gameObject.CompareTag(reflectTag) || collision.contactCount == 0)
        {
            return;
        }

        Vector2 currentVelocity = GetVelocity();
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflectedVelocity = Vector2.Reflect(currentVelocity, normal);

        SetVelocity(reflectedVelocity * reflectPower);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector2 GetVelocity()
    {
#if UNITY_6000_0_OR_NEWER
        return rb.linearVelocity;
#else
        return rb.velocity;
#endif
    }

    private void SetVelocity(Vector2 velocity)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = velocity;
#else
        rb.velocity = velocity;
#endif
    }
}
