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

    private BoxCollider2D boxCol;


    private void Awake()
    {
        if(render == null)render = GetComponent<SpriteRenderer>();
        if(renderText == null)renderText = GetComponent<TextMeshPro>();

        boxCol = GetComponent<BoxCollider2D>();

        OnClickAction += () => OnClick?.Invoke();
    }

    private void Update()
    {
        //GetObjectOver();
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
        if(!GetObjectOver() && canOverObject == false) return;
        if(EventSystem.current.IsPointerOverGameObject() == true && canOverObject == false) return;
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
    }
    public void OnMouseEnter()
    {
        if(!GetObjectOver() && canOverObject == false) return;
        if(EventSystem.current.IsPointerOverGameObject() == true && canOverObject == false) return;

        if(!interactable) return;
        if (!changingColor)
        {
            changingColor = true;
            gameObject.LeanColor(hoverColor, colorChangeTime).setOnComplete(() => changingColor = false);
        }
    }
    public void OnMouseExit()
    {  
        if(EventSystem.current.IsPointerOverGameObject() == true && canOverObject == false) return;
        if(!interactable) return;
        changingColor = true;
        LeanTween.value(gameObject, 10,10,1f);
        gameObject.LeanColor(defaultColor, colorChangeTime).setOnComplete(() => changingColor = false);
    }

    public bool GetObjectOver()
    {
        
        bool result = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition);
        
        float width  = boxCol.size.x;

        float height = boxCol.size.y;


        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(width, height), 0f);
        
        if(hit != null)
        {
            Debug.Log("Size of " + hit.name + "   " + boxCol.size);
            if(hit.gameObject != this.gameObject)
            {
                result = false;
            }
        }
        return result;
    }
}
