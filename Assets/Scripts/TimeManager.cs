using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public bool timeIsRunning;
    [SerializeField] private float dayDuration;

    private float dayTimer;
    private int days;
    private int months;

    [SerializeField]private TextMeshProUGUI dayText;

    public static TimeManager i;

    private void Awake() {
        i = this;
    }
    private void Start()
    {
        dayTimer = dayDuration;
        days = 1;
        months = 1;
    }

    private void Update() 
    {
        dayText.text = days.ToString("D2") + "/" + months.ToString("D2");

        if(!timeIsRunning) return;
        if(dayTimer <= 0f)
        {
            days++;
            if(days > 30)
            {
                months++;
                if(months > 12) months = 1;
                days = 1;
            }
            dayTimer = dayDuration;
        }else dayTimer -= Time.deltaTime;
    }
}
