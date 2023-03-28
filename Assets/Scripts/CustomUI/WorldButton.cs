using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private TextMeshPro renderText;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color clickedColor;

    [SerializeField] private float colorChangeTime;

    private bool changingColor;

    public UnityEvent OnClick;

    public System.Action OnClickAction;


    private void Awake()
    {
        if(render != null)render = GetComponent<SpriteRenderer>();
        if(renderText != null)renderText = GetComponent<TextMeshPro>();

        OnClickAction += () => OnClick?.Invoke();
    }


    public void OnMouseDown()
    {
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
        if (!changingColor)
        {
            changingColor = true;
            gameObject.LeanColor(hoverColor, colorChangeTime).setOnComplete(() => changingColor = false);
        }
        Debug.Log("Mouse Enter");
    }
    public void OnMouseExit()
    {  
        changingColor = true;
        LeanTween.value(gameObject, 10,10,1f);
        gameObject.LeanColor(defaultColor, colorChangeTime).setOnComplete(() => changingColor = false);
    
        Debug.Log("Mouse Exit");
    }
}
