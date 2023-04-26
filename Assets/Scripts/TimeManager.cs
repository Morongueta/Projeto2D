using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public bool timeIsRunning;
    [SerializeField] private float dayDuration;
    [SerializeField] private DayEvent[] events;

    private float dayTimer;
    [SerializeField]private int days;
    [SerializeField]private int months;
    
    private bool canCallEvents;
    [SerializeField]private TextMeshProUGUI dayText;

    public System.Action onTimeTick;
    public System.Action onMonthChangeTick;

    public static TimeManager i;

    private void Awake() {
        i = this;
    }
    private void Start()
    {
        dayTimer = dayDuration;
        canCallEvents = true;
    }

    private void Update() 
    {
        dayText.text = days.ToString("D2") + "/" + months.ToString("D2");

        if(!timeIsRunning) return;

        EventHandler();

        if(dayTimer <= 0f)
        {
            days++;
            if(days > 30)
            {
                onMonthChangeTick?.Invoke();
                months++;
                if(months > 12) months = 1;
                days = 1;
            }

            onTimeTick?.Invoke();
            canCallEvents = true;

            dayTimer = dayDuration;
        }else dayTimer -= Time.deltaTime;
    }



    public void EventHandler()
    {
        if(!canCallEvents) return;

        for (int i = 0; i < events.Length; i++)
        {
            DayEvent e = events[i];
            if(events[i].waitDays <= 0)
            {
                if((e.day == days && e.month == months) || (e.day == days && e.month == 0))
                {
                    e.onDayEvent?.Invoke();
                }
            }else{
                events[i].waitDays--;
            }
        }

        canCallEvents = false;
    }
}

[System.Serializable]
public struct DayEvent
{
    public string eventName;

    public int day;
    [Tooltip("Se for 0, pode ser qualquer mês")]
    public int month;

    public int waitDays;

    public UnityEngine.Events.UnityEvent onDayEvent;
}
