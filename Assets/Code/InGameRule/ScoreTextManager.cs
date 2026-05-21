using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreTextManager : MonoBehaviour
{
    /// <summary> 客側（クレーン操作側）の純損益を表示するテキスト </summary>
    [SerializeField] private TextMeshProUGUI _customerProfitText;
    /// <summary> 店側（プライズ操作側）の純損益を表示するテキスト </summary>
    [SerializeField] private TextMeshProUGUI _storeProfitText;

    /// <summary> テキスト点滅演出用のコルーチン（客側） </summary>
    private Coroutine _customerBlinkCoroutine;
    /// <summary> テキスト点滅演出用のコルーチン（店側） </summary>
    private Coroutine _storeBlinkCoroutine;
    /// <summary> UIテキストの基本色 </summary>
    private Color _baseTextColor = new(1f, 0.84f, 0.4f);
    
    /// <summary> 客側の現在の純損益（獲得景品額 - 投入金額） </summary>
    private int _customerTotalProfit;
    /// <summary> 店側の現在の純損益（投入金額 - 景品原価） </summary>
    private int _storeTotalProfit;

    public void OnCustomerAction(int cost)
    {
        _customerTotalProfit -= cost;
        _storeTotalProfit += cost;
        UpdateUI();

        if (_storeBlinkCoroutine != null) StopCoroutine(_storeBlinkCoroutine);
        _storeBlinkCoroutine = StartCoroutine(BlinkEffect(_storeProfitText, Color.red));
    }

    public void OnPrizeCaught(int prizeValue)
    {
        _customerTotalProfit += prizeValue;
        _storeTotalProfit -= prizeValue;
        UpdateUI();

        if (_customerBlinkCoroutine != null) StopCoroutine(_customerBlinkCoroutine);
        _customerBlinkCoroutine = StartCoroutine(BlinkEffect(_customerProfitText, Color.blue));
    }

    private void UpdateUI()
    {
        _customerProfitText.text = "客側損益: ¥" + _customerTotalProfit.ToString();
        _storeProfitText.text = "店側損益: ¥" + _storeTotalProfit.ToString();
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
