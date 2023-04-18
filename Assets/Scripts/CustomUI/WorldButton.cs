using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
    public bool canOverObject = false;
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
        GetObjectOver();
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
        if(EventSystem.current.IsPointerOverGameObject() == true && !GetObjectOver() && canOverObject == false) return;
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
        if(EventSystem.current.IsPointerOverGameObject() == true && !GetObjectOver() && canOverObject == false) return;
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
        if(EventSystem.current.IsPointerOverGameObject() == true && canOverObject == false) return;
        if(!interactable) return;
        changingColor = true;
        LeanTween.value(gameObject, 10,10,1f);
        gameObject.LeanColor(defaultColor, colorChangeTime).setOnComplete(() => changingColor = false);
    
        Debug.Log("Mouse Exit");
    }

    public bool GetObjectOver()
    {
        bool result = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(mousePos.x,mousePos.y,10), Vector3.forward, 10f);
        
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.gameObject != this.gameObject)
            {
                result = false;
            }
        }
        return result;
    }
}
