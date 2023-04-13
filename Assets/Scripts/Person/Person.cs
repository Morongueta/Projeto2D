using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Personality
{
    CALM,
    AGGRESSIVE
}

public class Person : MonoBehaviour
{
    public Action inFrontEvent;
    public Action goingAwayEvent;

    public UnityEvent inFrontUnityEvent;
    public UnityEvent goingAwayUnityEvent;

    protected PersonInfo info;

    protected Personality personality;

    private void Awake()
    {
        info = GetComponent<PersonInfo>();
    }

    private void Start()
    {
        SetupEvent();
    }

    public virtual void SetupEvent()
    {
        inFrontEvent += () => {inFrontUnityEvent?.Invoke();};
        goingAwayEvent += () => {goingAwayUnityEvent?.Invoke();};
    }

    public virtual void CallInFrontEvent()
    {
        inFrontEvent?.Invoke();
    }
    public virtual void CallGoingAwayEvent()
    {
        goingAwayEvent?.Invoke();
    }
}
