using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ArrowWriter : MonoBehaviour
{
    [SerializeField] private ArrowRenderer _arrow; // Inspectorで設定
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_arrow == null)
        {
            Debug.Log("ArrowRenderer is not assigned");
        }
    }

    public void Launch(Vector2 force)
    {
        _arrow.Show();
        _arrow.DrawArrow(force);

        _rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(HideArrowLater());
    }

    private IEnumerator HideArrowLater()
    {
        yield return new WaitForSeconds(0.5f);
        _arrow.Hide();
    }
}
