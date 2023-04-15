using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private TextMeshPro renderText;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color clickedColor = Color.white;
    [SerializeField] private Color disabledColor = Color.white;

    [SerializeField] private float colorChangeTime = 0.15f;

    public bool interactable = true;
    private bool lastInteractable = false;

    private bool changingColor;

    public UnityEvent OnClick;

    public System.Action OnClickAction;


    private void Awake()
    {
        if(render == null)render = GetComponent<SpriteRenderer>();
        if(renderText == null)renderText = GetComponent<TextMeshPro>();

        OnClickAction += () => OnClick?.Invoke();
    }

    private void Update()
    {
        if(!interactable)
        {
            if (!changingColor && render.color != disabledColor)
            {
                changingColor = true;
                gameObject.LeanColor(disabledColor, colorChangeTime).setOnComplete(() =>
                {
                    changingColor = false;
                    lastInteractable = interactable;
                });
            }
            
        }else{
            if(lastInteractable != interactable)
            {
                if (!changingColor && render.color != defaultColor)
                {
                    changingColor = true;
                    gameObject.LeanColor(defaultColor, colorChangeTime).setOnComplete(() =>
                    {
                        changingColor = false;
                        lastInteractable = interactable;
                    });
                }
            }
        }
    }

    public void SetSprite(Sprite spr)
    {
        if(render != null)
        {
            render.sprite = spr;
        }
    }


    public void OnMouseDown()
    {
        if(!interactable) return;
        if (!changingColor)
        {
            changingColor = true;
            gameObject.LeanColor(clickedColor, colorChangeTime).setOnComplete(() =>
            {
                changingColor = false;
                OnMouseEnter();
            }
            );  
        }
        
        OnClickAction?.Invoke();

        Debug.Log("Clicked");
    }
    public void OnMouseEnter()
    {
        if(!interactable) return;
        if (!changingColor)
        {
            changingColor = true;
            gameObject.LeanColor(hoverColor, colorChangeTime).setOnComplete(() => changingColor = false);
        }
        Debug.Log("Mouse Enter");
    }
    public void OnMouseExit()
    {  
        if(!interactable) return;
        changingColor = true;
        LeanTween.value(gameObject, 10,10,1f);
        gameObject.LeanColor(defaultColor, colorChangeTime).setOnComplete(() => changingColor = false);
    
        Debug.Log("Mouse Exit");
    }
}
