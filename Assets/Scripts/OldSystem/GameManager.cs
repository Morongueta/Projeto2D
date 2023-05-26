using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int curDay { get { return day; } private set { day = value; onUpdateDay?.Invoke(); } }

    private int day;

    [SerializeField] private SpecialEvent[] specialEvents;

    private Action onUpdateDay;

    private void Start()
    {
        onUpdateDay += () => {
            for (int y = 0; y < specialEvents.Length; y++)
            {
                if (specialEvents[y].day == curDay )
                {
                    specialEvents[y].onStart?.Invoke();
                }
                if (specialEvents[y].day == curDay - 1)
                {
                    specialEvents[y].onFinish?.Invoke();
                }
            }
        };


        curDay = 1;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V)) curDay++;
    }

    public void ShowText(string text)
    {
        Debug.Log(text);
    }
}

[System.Serializable]
public class SpecialEvent
{
    public int day;
    public UnityEvent onStart;

    public UnityEvent onFinish;
}
