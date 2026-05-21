using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArrowRenderer : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private float _lineSize;
    [SerializeField] private float _lineWidth;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false; // 初期は非表示
    }

    public void DrawArrow(Vector2 force)
    {
        float magnitude = force.magnitude;
         float length = magnitude * _lineSize;

        Vector3 dir = force.normalized;
        Vector3 start = transform.position;
        Vector3 end = start + dir * length;

        line.SetPosition(1, start);
        line.SetPosition(0, end);

        float width = Mathf.Clamp(magnitude * 0.002f, 0.02f, 0.2f);
        line.startWidth = width;
        line.endWidth = width * _lineWidth;
    }

    public void Show()
    {
        line.enabled = true;
    }

    public void Hide()
    {
        line.enabled = false;
    }
}