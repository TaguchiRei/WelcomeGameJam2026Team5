using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    public static PrizeManager Instance;

    // 景品を置く場所（空オブジェクト）
    public Transform storagePoint;

    private void Awake()
    {
        Instance = this;
    }

     public void SendToStorage(GameObject prize)
    {
        if (storagePoint == null)
        {
            Debug.Log("storagePoint is not assigned");
            return;
        }
        // 物理を止める
        Rigidbody2D rb = prize.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 移動
        prize.transform.position = storagePoint.position;

        // 必要なら非表示
        // prize.SetActive(false);

        // 必要なら再利用のための初期化処理もここに書ける
    }
}
