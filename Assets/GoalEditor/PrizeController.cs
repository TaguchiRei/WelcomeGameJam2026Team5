using UnityEngine;

public class PrizeController : MonoBehaviour
{
    [SerializeField] private ArrowRenderer arrow; // Inspectorで設定
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 force)
    {
        arrow.Show();
        arrow.DrawArrow(force);

        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(HideArrowLater());
    }

    private System.Collections.IEnumerator HideArrowLater()
    {
        yield return new WaitForSeconds(0.5f);
        arrow.Hide();
    }
}
