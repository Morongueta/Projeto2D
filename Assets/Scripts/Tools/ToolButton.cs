using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public Tool tool;

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
