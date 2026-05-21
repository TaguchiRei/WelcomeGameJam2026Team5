using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArrowRenderer : MonoBehaviour
{
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false; // 初期は非表示
    }

    public void DrawArrow(Vector2 force)
    {
        float magnitude = force.magnitude;
        float length = magnitude * 0.01f;

        Vector3 dir = force.normalized;
        Vector3 start = transform.position;
        Vector3 end = start + dir * length;

        line.SetPosition(0, start);
        line.SetPosition(1, end);

        float width = Mathf.Clamp(magnitude * 0.002f, 0.02f, 0.2f);
        line.startWidth = width;
        line.endWidth = width * 0.5f;
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