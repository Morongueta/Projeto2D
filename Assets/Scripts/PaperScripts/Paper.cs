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

    
    private Vector3 paperScale;
    private Vector3 miniScale;

    private Rigidbody2D rb;
    private SpriteRenderer render;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        paperScale = transform.localScale;
        miniScale = paperScale * .5f;
    }

    private void Update()
    {
        BoxInteraction();

        Gravity();
    }

    private void BoxInteraction()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f,Vector2.zero,1f, boxLayer);

        if(hit.collider != null)
        {
            if(GetComponent<Draggable>().holding) transform.localScale = miniScale;
        }else{
            transform.localScale = paperScale;
        }
    }

    public void ResetSize()
    {
        transform.localScale = paperScale;
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
