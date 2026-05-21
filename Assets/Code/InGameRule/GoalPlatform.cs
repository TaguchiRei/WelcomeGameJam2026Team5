using UnityEngine;

public class GoalPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PrizeManager.Instance == null)
        {
            Debug.Log("PrizeManager is not assigned");
            return;
        }

        if (!collision.CompareTag("Prize")) return;

        // PrizeDataHolder を取得
        PrizeDataHolder holder = collision.GetComponent<PrizeDataHolder>();
        if (holder == null || holder.Data == null)
        {
            Debug.Log("PrizeDataHolder or PrizeData is missing");
            return;
        }

        PrizeData data = holder.Data;

        // 景品専用SEを再生
        PlayPrizeSE(data);

        // スコア処理
        ScoreTextManager gm = FindFirstObjectByType<ScoreTextManager>();
        if (gm != null)
        {
            gm.OnPrizeCaught(data.Price);
        }

        // 景品移動
        PrizeManager.Instance.SendToStorage(collision.gameObject);
    }

    private void PlayPrizeSE(PrizeData data)
    {
        if (data.PrizeSE == null)
        {
            Debug.Log("Prize SE is not assigned");
            return;
        }

        // SoundEffectPool が AudioClip 再生に対応している前提
        SoundEffectPool.Instance.GetSeObject().Play(data.PrizeSE);
    }
}
