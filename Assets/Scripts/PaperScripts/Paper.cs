using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PaperType
{
    NONE,
    WARNING,
    CONTRACT,
    HIRE
}
public class Paper : MonoBehaviour
{
    public PaperType type;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private Vector2 minPaperSpace;
    [SerializeField] private Vector2 maxPaperSpace;

    [SerializeField] private float fallSpeed;

    [SerializeField] private bool fixedOn;

    private bool lockGravity = false;

    
    private Vector3 paperScale;
    private Vector3 miniScale;

    private float lineSize;

    private Rigidbody2D rb;
    private SpriteRenderer render;
    private Drawable draw = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();
        draw = GetComponent<Drawable>();
    }

    private void Start()
    {
        paperScale = transform.localScale;
        miniScale = paperScale * .5f;
    }

    private void Update()
    {
        BoxInteraction();

        if (lockGravity)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            Gravity();
        }
    }

    private void BoxInteraction()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f,Vector2.zero,1f, boxLayer);

        if(draw != null)
            if(lineSize == 0)
            {
                LineRenderer l = GetComponentInChildren<LineRenderer>();
                if(l != null) lineSize = l.startWidth;
            } 

        if(hit.collider != null)
        {
            if(GetComponent<Draggable>().holding)
            {
                transform.localScale = miniScale;
                ChangeLineSize(lineSize / 2f);
            }
        }else{
            transform.localScale = paperScale;
            ChangeLineSize(lineSize);
        }
    }

    public void ResetSize()
    {
        transform.localScale = paperScale;
        ChangeLineSize(lineSize);
    }

    public void ReleasePaper()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f,Vector2.zero,1f, boxLayer);
        if(hit.collider != null)
        {
            PaperBox box = hit.collider.GetComponent<PaperBox>();
            if(box != null)
            {
                box.AddToBox(this.gameObject);
            }
        }
    }

    private void Gravity()
    {
        if(transform.position.y > maxPaperSpace.y - render.bounds.size.y / 2f)
        {
            rb.gravityScale = (rb.velocity.y > 0f) ? fallSpeed / 2f : fallSpeed;
        }else{
            rb.gravityScale = 0f;
        }
    }

    public void LockGravity(bool to)
    {
        lockGravity = to;
    }

    private void ChangeLineSize(float size)
    {
        if(draw == null) return;

        LineRenderer[] lines = draw.GetLines();

        if(lines.Length == 0) return;

        for(int i = 0; i < lines.Length; i++)
        {
            lines[i].startWidth = size;
            lines[i].endWidth = size;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 downLeft = new Vector2(minPaperSpace.x, minPaperSpace.y);
        Vector2 downRight = new Vector2(maxPaperSpace.x, minPaperSpace.y);
        Vector2 upLeft = new Vector2(minPaperSpace.x, maxPaperSpace.y);
        Vector2 upRight = new Vector2(maxPaperSpace.x, maxPaperSpace.y);

        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(downRight, upRight);
        Gizmos.DrawLine(upRight, upLeft);
        Gizmos.DrawLine(downLeft, upLeft);
    }
}
