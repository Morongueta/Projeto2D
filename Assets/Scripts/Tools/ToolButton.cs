using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public Tool tool;
    [Header("Pen")]
    public PenProperties pen;

    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color col)
    {
        render.color = new Color(render.color.r,render.color.g,render.color.b, col.a);
    }
}

[System.Serializable]
public struct PenProperties
{
    public GameObject penObject;
    public Sprite penMouseSprite;
    public bool isErasable;
}
