using UnityEngine;

[CreateAssetMenu(fileName = "PrizeData", menuName = "GoalEditor/PrizeData", order = 1)]
public class PrizeData : ScriptableObject
{
    [Header("景品の基本情報")] 
    public string PrizeName = "ぬいぐるみ";
    public int Price = 500;
}