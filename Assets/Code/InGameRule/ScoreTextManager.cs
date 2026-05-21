using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreTextManager : MonoBehaviour
{
    /// <summary> 客側（クレーン操作側）のスコアを表示するテキスト </summary>
    [SerializeField] private TextMeshProUGUI _customerProfitText;
    /// <summary> 店側（プライズ操作側）のスコアを表示するテキスト </summary>
    [SerializeField] private TextMeshProUGUI _storeProfitText;

    /// <summary> テキスト点滅演出用のコルーチン（客側） </summary>
    private Coroutine _customerBlinkCoroutine;
    /// <summary> テキスト点滅演出用のコルーチン（店側） </summary>
    private Coroutine _storeBlinkCoroutine;
    /// <summary> UIテキストの基本色 </summary>
    private Color _baseTextColor = new(1f, 0.84f, 0.4f);
    
    /// <summary> 客側の現在のスコア（獲得景品額の合計） </summary>
    private int _customerTotalProfit;
    public int CustomerTotalScore => _customerTotalProfit;

    /// <summary> 店側の現在のスコア（投入金額の合計） </summary>
    private int _storeTotalProfit;
    public int StoreTotalScore => _storeTotalProfit;

    public void OnCustomerAction(int cost)
    {
        // 店側のスコアのみ加算する
        _storeTotalProfit += cost;
        UpdateUI();

        if (_storeBlinkCoroutine != null) StopCoroutine(_storeBlinkCoroutine);
        _storeBlinkCoroutine = StartCoroutine(BlinkEffect(_storeProfitText, Color.red));
    }

    public void OnPrizeCaught(int prizeValue)
    {
        // 客側のスコアのみ加算する
        _customerTotalProfit += prizeValue;
        UpdateUI();

        if (_customerBlinkCoroutine != null) StopCoroutine(_customerBlinkCoroutine);
        _customerBlinkCoroutine = StartCoroutine(BlinkEffect(_customerProfitText, Color.blue));
    }

    private void UpdateUI()
    {
        _customerProfitText.text = "客側スコア: ¥" + _customerTotalProfit.ToString();
        _storeProfitText.text = "店側スコア: ¥" + _storeTotalProfit.ToString();
    }

    IEnumerator BlinkEffect(TextMeshProUGUI targetText, Color flashColor)
    {
        Color originalColor = _baseTextColor;

        for (int i = 0; i < 3; i++)
        {
            targetText.color = flashColor;
            yield return new WaitForSeconds(0.1f);

            targetText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    void Start()
    {
        _customerProfitText.color = _baseTextColor;
        _storeProfitText.color = _baseTextColor;
        UpdateUI();
    }
}
