using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouse : MonoBehaviour
{
    [SerializeField] private Sprite defaultHand;
    [SerializeField] private Sprite clickHand;
    [SerializeField] private Sprite eraserHand;
    [SerializeField] private Sprite penHand;

    [SerializeField] private LayerMask customUILayer;

    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        DrawMouse();
    }


    private void DrawMouse()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;

        Sprite handSprite = null;

        switch (ToolManager.i.GetTool())
        {
            case Tool.HAND:
                handSprite = (ToolManager.i.UsingHand) ? clickHand : defaultHand;
                break;
            case Tool.ERASER:
                handSprite = eraserHand;
                break;
            case Tool.PEN:
                handSprite = penHand;
                break;
        }

        Cursor.visible = (handSprite == null);

        render.sprite = handSprite;
    }
}
