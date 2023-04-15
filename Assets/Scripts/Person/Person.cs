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

    [Header("Chat Button")]
    [SerializeField]protected Vector3 buttonPosition;
    [SerializeField]protected WorldButton chatButton;
    [SerializeField]protected Sprite defaultSprite;
    [SerializeField]protected Sprite closeSprite;

    private void Awake()
    {
        info = GetComponent<PersonInfo>();
    }

    private void Start()
    {
        
        chatButton.SetSprite(defaultSprite);
        chatButton.gameObject.SetActive(false);
        height = transform.position.y;
        walkTimer = UnityEngine.Random.Range(0f, 10f);
        SetupEvent();
    }

    private void Update()
    {
        chatButton.transform.position = buttonPosition;

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
        goingAwayEvent += () => {goingAwayUnityEvent?.Invoke(); chatButton.gameObject.SetActive(false); };

        
    }

    public virtual void SetupChatButton()
    {
        chatButton.OnClickAction += () => {

            if(!TextBoxManager.i.showingTextBox)
            {
                chatButton.SetSprite(closeSprite);
            }else{
                chatButton.SetSprite(defaultSprite);
            } 
        };
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
