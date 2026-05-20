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
        
        PrizeData data = collision.GetComponent<PrizeData>();
        if (data == null) return;
        
        ScoreTextManager gm = FindFirstObjectByType<ScoreTextManager>();
        if (gm != null)
        {
            gm.AddScoreA(300);
        }
        
        PrizeManager.Instance.SendToStorage(collision.gameObject);
    }
}
