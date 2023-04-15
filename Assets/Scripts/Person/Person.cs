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

    private float height;

    [HideInInspector]public bool walking = false;

    protected float walkTimer;

    private void Awake()
    {
        info = GetComponent<PersonInfo>();
    }

    private void Start()
    {
        height = transform.position.y;
        walkTimer = UnityEngine.Random.Range(0f, 10f);
        SetupEvent();
    }

    private void Update()
    {
        if(walking)
        {
            walkTimer += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, height + Mathf.Sin(walkTimer * 20f) / 4f);
        }
        else
        {
            transform.position = new Vector2(transform.position.x,height);
        }
    }

    public PersonInfo GetInfo()
    {
        return info;
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
