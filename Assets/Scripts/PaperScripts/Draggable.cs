using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool active = true;

    [SerializeField] private bool lockIn = false;
    [SerializeField] private Vector2 minPaperSize;
    [SerializeField] private Vector2 maxPaperSize;
    [SerializeField] private bool canMoveLayer = true;

    public Action OnSelect;

    public bool holding = false;

    private SpriteRenderer rend;
    private Collider2D col;

    public static List<Draggable> allDraggables = new List<Draggable>();

    private void Awake()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        allDraggables.Add(this);
    }

    private void Start()
    {
        GoForward();
        GoBackward();
    }

    private void Update()
    {
        col.enabled = active;
        if (!active) return;
        float width = rend.bounds.size.x / 2f;
        float height = rend.bounds.size.y / 2f;

        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, minPaperSize.x + width, maxPaperSize.x - width );
        pos.y = Mathf.Clamp(transform.position.y, minPaperSize.y + height, maxPaperSize.y - height);
        transform.position = pos;
    }

    public void GoForward()
    {
        if (!canMoveLayer) return;
        SpriteRenderer[] rend = GetComponentsInChildren<SpriteRenderer>();
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        LineRenderer[] lines = GetComponentsInChildren<LineRenderer>();

        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].sortingOrder = 10;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].sortingOrder = 10;
        }
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].sortingOrder = 10;
        }
    }

    public void GoBackward()
    {
        if (!canMoveLayer) return;
        for (int y = 0; y < allDraggables.Count; y++)
        {
            if(allDraggables[y] != null)
            {
                if (allDraggables[y].active && allDraggables[y].canMoveLayer)
                {
                    SpriteRenderer[] rends = allDraggables[y].GetComponentsInChildren<SpriteRenderer>();
                    TextMeshPro[] texts = allDraggables[y].GetComponentsInChildren<TextMeshPro>();
                    LineRenderer[] lines = allDraggables[y].GetComponentsInChildren<LineRenderer>();

                    Debug.Log("GoBackwards " + allDraggables[y].name);

                    for (int i = 0; i < rends.Length; i++)
                    {
                        rends[i].sortingOrder--;
                    }

                    for (int i = 0; i < texts.Length; i++)
                    {
                        texts[i].sortingOrder--;
                    }
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i].sortingOrder--;
                    }
                }
            }

        }
    }

    public void SetLayer(int layer)
    {
        if (!canMoveLayer) return;
        SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer>();
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        LineRenderer[] lines = GetComponentsInChildren<LineRenderer>();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].sortingOrder = layer;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].sortingOrder = layer;
        }
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].sortingOrder = layer;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 downLeft = new Vector2(minPaperSize.x, minPaperSize.y);
        Vector2 downRight = new Vector2(maxPaperSize.x, minPaperSize.y);
        Vector2 upLeft = new Vector2(minPaperSize.x, maxPaperSize.y);
        Vector2 upRight = new Vector2(maxPaperSize.x, maxPaperSize.y);

        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(downRight, upRight);
        Gizmos.DrawLine(upRight, upLeft);
        Gizmos.DrawLine(downLeft, upLeft);
    }

}
