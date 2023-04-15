using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gatto.Utils
{
    public class _PeriodTimer : MonoBehaviour
{   
    public int ID;
    public string situation;
    public float timer;
    public float maxTimer;
    private bool finished;

    public Action OnEndTimer;
    public Action<float,float> OnTimerTick;

    



    public void AddEndEvent(Action addition)
    {
        OnEndTimer += addition;
    }

    public void ChangeTickEvent(Action<float,float> OnTimerTick)
    {
        this.OnTimerTick = OnTimerTick;
    }

    public void ShutDown(bool executeEndEvent = false)
    {
        if(executeEndEvent) OnEndTimer?.Invoke();

        Destroy(this.gameObject);
    }


    void Update()
    {
        OnTimerTick?.Invoke(timer,maxTimer);
        gameObject.name = situation + " " + Mathf.Ceil(timer);

        if(timer <= 0f)
        {
            if(finished)
                return;
            OnEndTimer?.Invoke();
            Destroy(this.gameObject, 1f);
            finished = true;
        }else{
            timer -= Time.deltaTime;
        }
    }
}


public static class PeriodTimer
{
    public static List<_PeriodTimer> allTimers;
    public static _PeriodTimer Timer(float time, Action OnEndTimer, Action<float,float> OnTimerTick = null, string situation = "_TimerOf", int id = 0, bool unique = false)
    {
        if(allTimers == null) allTimers = new List<_PeriodTimer>();
        
        if(unique)
        {
            foreach(_PeriodTimer t in allTimers)
            {
                if(t.ID == id)
                {
                    //Debug.Log("There is a version of this ID: " + id);
                    return null;
                }
            }
        }

        GameObject newTimer = new GameObject(situation + " " + time);
        _PeriodTimer timer = newTimer.AddComponent<_PeriodTimer>();

        OnEndTimer += () => allTimers.Remove(timer);
        allTimers.Add(timer);

        timer.ID = id;
        timer.situation = situation;
        timer.timer = time;
        timer.maxTimer = time;
        timer.OnEndTimer = OnEndTimer;
        timer.OnTimerTick = OnTimerTick;

        return timer;
    }
    

    //public static GameObject Timer(_PeriodTimer copy);
}
}
