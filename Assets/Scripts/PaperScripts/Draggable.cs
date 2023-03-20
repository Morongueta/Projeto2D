using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Vector2 minPaperSize;
    [SerializeField] private Vector2 maxPaperSize;

    public bool holding = false;

    private Vector3 paperScale;
    private Vector3 miniScale;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponentInChildren<SpriteRenderer>();

        
    }

    private void Start()
    {
        paperScale = transform.localScale;
        miniScale = paperScale * .75f;
        GoForward();
        GoBackward();
    }

    private void Update()
    {
        float width = rend.bounds.size.x / 2f;
        float height = rend.bounds.size.y / 2f;

        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, minPaperSize.x + width, (holding) ? transform.position.x : maxPaperSize.x - width );
        pos.y = Mathf.Clamp(transform.position.y, minPaperSize.y + height, maxPaperSize.y - height);
        transform.position = pos;

        if(transform.position.x > maxPaperSize.x)
        {
            transform.localScale = miniScale;
        }else{
            transform.localScale = paperScale;
        }
    }

    public void GoForward()
    {
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
        SpriteRenderer[] rends = FindObjectsOfType<SpriteRenderer>();
        TextMeshPro[] texts = FindObjectsOfType<TextMeshPro>();
        LineRenderer[] lines = FindObjectsOfType<LineRenderer>();

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
