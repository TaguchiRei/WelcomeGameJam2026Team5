using UnityEngine;

public class PrizeController : MonoBehaviour
{
    [SerializeField] private string _prizeTag = "PrizeTag";

    // つかまれている　状態判定
    public bool IsCaught = false;
    // クレーンのtransformを引数で受け取る
    // クレーンの子オブジェクトにする　同期

    /// <summary>
    /// つかまれたら停止する
    /// クレーンのtransformを引数で受け取る
    /// クレーンの子オブジェクトにする　同期
    /// </summary>
    private void PrizeStop()
    {
        IsCaught = true;
    }

    /// <summary>
    /// つかまれた状態を解除する
    /// クレーンの子オブジェクトから外す
    /// 物理挙動を再開する
    /// </summary>
    private void PrizeRelease()
    {
        IsCaught = false;
    }

    /// <summary>
    /// つかまれたオブジェクトにPrizeTagがついていたら解除する
    /// </summary>
    public void PrizeRelease(GameObject grabbedObject)
    {
        if (!grabbedObject.CompareTag(_prizeTag))
        {
            return;
        }
        PrizeRelease();
    }
}
