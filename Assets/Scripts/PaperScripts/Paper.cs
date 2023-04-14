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
    private bool inBox;
    [SerializeField] private LayerMask paperDestroyerLayer;
    private bool inDestroyer;
    [SerializeField] private Vector2 minPaperSpace;
    [SerializeField] private Vector2 maxPaperSpace;

    [SerializeField] private float fallSpeed;

    [SerializeField] private bool fixedOn;

    private bool lockGravity = false;

    
    private Vector3 paperScale;
    private Vector3 miniScale;

    private List<float> lineSize = new List<float>();

    private Rigidbody2D rb;
    private SpriteRenderer render;
    private Drawable draw = null;

    public bool canGet = true;

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
        DestroyerInteraction();

        if (lockGravity)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            Gravity();
        }
    }

    private void DestroyerInteraction()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f,Vector2.zero,1f, paperDestroyerLayer);

        inDestroyer = (hit.collider != null);
    }

    private void BoxInteraction()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f,Vector2.zero,1f, boxLayer);

        inBox = (hit.collider != null);

        if(draw != null)
        {
            LineRenderer[] lines = draw.GetLines();
            if(lineSize.Count != lines.Length)
            {
                lineSize.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    if(lines[i] == null)
                    {
                        lineSize.Add(0f);
                    }else{
                        lineSize.Add(lines[i].startWidth);
                    }
                    
                }
            } 
        }

        if(inBox)
        {
            if(GetComponent<Draggable>().holding)
            {
                transform.localScale = miniScale;

                ChangeLineSize(lineSize.ToArray());
            }
        }else{
            transform.localScale = paperScale;
            ChangeLineSize(lineSize.ToArray());
        }
    }

    public void ResetSize()
    {
        transform.localScale = paperScale;
        ChangeLineSize(lineSize.ToArray());
    }

    public void ReleasePaper()
    {
        if(inDestroyer)
        {
            QueueManager.i.RemoveFromQueueCurriculum(GetComponent<Curriculum>().curriculumData);
            Destroy(this.gameObject);
        }

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

    private void ChangeLineSize(float[] sizes)
    {
        if(draw == null) return;

        LineRenderer[] lines = draw.GetLines();

        if(lines.Length == 0) return;

        for(int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == null) return;
            lines[i].startWidth = sizes[i];
            lines[i].endWidth = sizes[i];
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
