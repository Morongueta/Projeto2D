using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Gatto.Utils;

public class EarningSystem : MonoBehaviour
{
    [Header("Tooltip")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private float textHeight;
    [SerializeField] private float textWidth;
    [SerializeField] private TextMeshProUGUI tooltipMoney;
    [SerializeField] private Transform tooltipArea;
    [SerializeField] private RectTransform background;
    private List<TextMeshProUGUI> tooltipTexts = new List<TextMeshProUGUI>();
    private List<MoneyTip> tooltipShown = new List<MoneyTip>();

    public struct MoneyTip{
        public string reason;
        public int amount;
    }

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int startMoney;
    private int money;

    public static EarningSystem i;

    private void Awake() {
        i = this;
    }

    private void Start() 
    {
        money = startMoney;
        UpdateTooltip();
    }

    private void Update() {
        moneyText.text = "R$" + money.ToString("N0");

        if(Input.GetKeyDown(KeyCode.G))ChangeMoney(Random.Range(-1000,1000), "NoReason");
    }   

    public int GetMoney()
    {
        return money;
    }


    public void ChangeMoney(int amount, string reason = "")
    {
        money += amount;

        AddTooltip(amount, reason);
    }


    #region Tooltip

    public void AddTooltip(int amount, string reason = "")
    {
        MoneyTip tip = new MoneyTip();
        tip.amount = amount;
        tip.reason = reason;

        tooltipShown.Add(tip);
        PeriodTimer.Timer(3f, ()=>{
            tooltipShown.Remove(tip);
            UpdateTooltip();
        });

        UpdateTooltip();
    }

    private void UpdateTooltip()
    {
        for (int i = 0; i < tooltipTexts.Count; i++)
        {
            Destroy(tooltipTexts[i].gameObject);
        }

        tooltipTexts.Clear();

        for (int i = 0; i < tooltipShown.Count; i++)
        {
            GameObject newText = Instantiate(tooltipMoney.gameObject, tooltipArea);
            TextMeshProUGUI text = newText.GetComponent<TextMeshProUGUI>();

            text.text = tooltipShown[i].reason + ": " + tooltipShown[i].amount.ToString("N0");
            text.color = (tooltipShown[i].amount < 0) ? Color.red : Color.green;

            tooltipTexts.Add(text);
        }

        Rect rect = background.rect;
        rect.width = (textWidth * mainCanvas.renderingDisplaySize.x / 1366);
        rect.height = (textHeight * mainCanvas.renderingDisplaySize.y / 768) * tooltipShown.Count;
        background.LeanSize(new Vector2(rect.width, rect.height), .05f);
    }

    #endregion

}
