using UnityEngine;

public class PrizeController : MonoBehaviour
{
    // つかまれている　状態判定
    public bool isKinematic = false;
    // クレーンのtransformを引数で受け取る
    // クレーンの子オブジェクトにする　同期

    /// <summary>
    ///　つかまれたら停止する
    /// クレーンのtransformを引数で受け取る
    /// クレーンの子オブジェクトにする　同期
    /// </summary>
    private void PrizeStop()
    {
        isKinematic = true;
    }

    /// <summary>
    /// つかまれた状態を解除する
    /// クレーンの子オブジェクトから外す
    /// 物理挙動を再開する
    /// </summary>
    private void PrizeRelease()
    {
        isKinematic = false;
    }
}
