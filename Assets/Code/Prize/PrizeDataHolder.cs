using UnityEngine;

public class PrizeDataHolder : MonoBehaviour
{
    [SerializeField] private PrizeData prizeData;

    public PrizeData Data => prizeData;
}
