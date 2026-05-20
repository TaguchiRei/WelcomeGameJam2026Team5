using UnityEngine;

public class PrizeOperation : MonoBehaviour
{
    [SerializeField] private float forcePower = 5.0f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 mouseDownPosition;

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
        mouseDownPosition = GetMouseWorldPosition();
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
        Vector2 mouseUpPosition = GetMouseWorldPosition();
        Vector2 force = mouseDownPosition - mouseUpPosition;

        rb.AddForce(force * forcePower, ForceMode2D.Impulse);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
