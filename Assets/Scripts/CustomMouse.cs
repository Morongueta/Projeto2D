using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouse : MonoBehaviour
{
    [SerializeField] private Sprite defaultHand;
    [SerializeField] private Sprite pointingHand;
    [SerializeField] private Sprite clickHand;
    [SerializeField] private Sprite eraserHand;
    [SerializeField] private Sprite penHand;
    [SerializeField] private LayerMask customUILayer;

    public bool pointing;

    private SpriteRenderer render;

    public static CustomMouse i;
    private void Awake()
    {
        i = this;
        render = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        pointing = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), .15f, Vector2.zero, .15f, customUILayer).collider != null;
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
        if(pointing) handSprite = pointingHand;
        Cursor.visible = (handSprite == null);

        render.sprite = handSprite;
    }
}
