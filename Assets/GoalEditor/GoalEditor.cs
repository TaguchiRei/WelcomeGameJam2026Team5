using UnityEngine;

public class GoalEditor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PrizeManager.Instance == null)
        {
            Debug.Log("PrizeManager is not assigned");
            return;
        }
        //if (UIManager.Instance == null)
        //{
        //    Debug.Log("UIManager is not assigned");
        //    return;
        //}

        if (!collision.CompareTag("Prize")) return;

        PrizeData data = collision.GetComponent<PrizeData>();
        if (data == null) return;

        // UIへ通知
        //UIManager.Instance.OnPrizeGet(data);

        // 景品を回収場所へ移動
        PrizeManager.Instance.SendToStorage(collision.gameObject);

    }
}
