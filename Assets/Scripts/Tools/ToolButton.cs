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
        render.color = col;
    }
}

[System.Serializable]
public struct PenProperties
{
    public GameObject penObject;
    public bool isErasable;
}
