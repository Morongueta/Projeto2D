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
        Debug.Log("Clicked");
        if (!GetObjectOver() && canOverObject == false) return;
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


        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 1f);

        List<RaycastHit2D> organizedHits = new List<RaycastHit2D>();
        
        organizedHits.AddRange(hits);

        if(organizedHits.Count == 0) return true;

        for (int i = 1; i < organizedHits.Count; i++)
        {
            SpriteRenderer renderer = organizedHits[i].collider.GetComponent<SpriteRenderer>();

            if(renderer != null)
            {
                SpriteRenderer rendererLast = organizedHits[i - 1].collider.GetComponent<SpriteRenderer>();
                if(rendererLast != null)
                {
                    int lastID = SortingLayer.GetLayerValueFromID(rendererLast.sortingLayerID);
                    int curID = SortingLayer.GetLayerValueFromID(renderer.sortingLayerID);
                    if(lastID < curID)
                    {
                        RaycastHit2D last = organizedHits[i - 1];
                        RaycastHit2D current = organizedHits[i];

                        organizedHits[i] = last;
                        organizedHits[i - 1] = current;
                        i = 0;
                    }
                }
            }
        }

        for (int i = 1; i < organizedHits.Count; i++)
        {
            SpriteRenderer renderer = organizedHits[i].collider.GetComponent<SpriteRenderer>();

            if(renderer != null)
            {
                SpriteRenderer rendererLast = organizedHits[i - 1].collider.GetComponent<SpriteRenderer>();
                if(rendererLast != null)
                {
                    if(rendererLast.sortingLayerID == renderer.sortingLayerID)
                    {
                        if(rendererLast.sortingOrder < renderer.sortingOrder)
                        {
                            RaycastHit2D last = organizedHits[i - 1];
                            RaycastHit2D current = organizedHits[i];

                            organizedHits[i] = last;
                            organizedHits[i - 1] = current;
                            i = 0;
                        }
                    }
                }
            }
        }
        
        for (int i = 0; i < organizedHits.Count; i++)
        {
            SpriteRenderer renderer = organizedHits[i].collider.GetComponent<SpriteRenderer>();
            int id = SortingLayer.GetLayerValueFromID(renderer.sortingLayerID);
            Debug.Log("The object named: " + renderer.name + " is on layer: " + id + " at order: " + renderer.sortingOrder);
        }

        if(organizedHits[0].collider != null)
        {
            if(organizedHits[0].collider.gameObject != this.gameObject)
            {
                result = false;
            }
        }
        return result;
    }
}
