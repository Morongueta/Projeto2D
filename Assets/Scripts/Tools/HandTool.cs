using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTool : MonoBehaviour
{
    [SerializeField]private LayerMask dragLayer;
    private Vector2 mousePos;
    private Vector2 cameraOffset;
    private Vector2 startMousePos;
    private Vector2 objectPos;
    private Vector2 pos;

    private Vector2 lastPos;
    private GameObject objectDown;

    private SpriteRenderer objectRender;
    private Rigidbody2D objectRB;

    public bool released;
    public static HandTool i;

    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if(ToolManager.i.GetTool() != Tool.HAND) return;

        mousePos = Input.mousePresent ? Input.mousePosition : Vector2.zero;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(mousePos);

        ToolManager.i.UsingHand = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Mouse click
        {
            cameraOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(mouse, Vector2.zero, 1f,dragLayer);
            if(hit.Length == 0) return;
            RaycastHit2D closest = hit[0];
            int close = -9999999;
            for (int i = 0; i < hit.Length; i++)
            {
                if(hit[i].collider.GetComponentInChildren<SpriteRenderer>().sortingOrder > close)
                {
                    closest = hit[i];
                    close = hit[i].collider.GetComponentInChildren<SpriteRenderer>().sortingOrder;
                }
            }
            if(closest.collider != null)
            {
                Draggable drag = closest.collider.GetComponent<Draggable>();
                if(drag != null)
                {
                    objectPos = drag.transform.position;
                    objectDown = drag.gameObject;
                    objectDown.GetComponent<Draggable>().GoForward();
                    objectDown.GetComponent<Draggable>().holding = true;
                    objectRender = objectDown.GetComponentInChildren<SpriteRenderer>();
                    objectRB = objectDown.GetComponent<Rigidbody2D>();
                    objectDown.GetComponent<Draggable>().OnSelect?.Invoke();
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && objectDown != null)
        {
            
            pos = (mouse - (Vector3)cameraOffset) + (Vector3)objectPos;

            lastPos = pos;

        }

        if(Input.GetKeyUp(KeyCode.Mouse0) && objectDown != null) 
        {
            UpFuction();
        }
    }

    private void FixedUpdate()
    {
        if(objectRB != null)
        {
            Vector3 dir = pos - objectRB.position;
            Vector3 speed = dir * 20f;
            objectRB.velocity = speed;
        }
    }

    public void TransferObject(GameObject to)
    {
        to.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cameraOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        objectDown.GetComponent<Draggable>().GoBackward();
        objectPos = to.transform.position;
        objectDown = to;
        objectDown.GetComponent<Draggable>().GoForward();
        objectDown.GetComponent<Draggable>().holding = true;
        objectRender = objectDown.GetComponentInChildren<SpriteRenderer>();
        objectRB = objectDown.GetComponent<Rigidbody2D>();
        objectDown.GetComponent<Draggable>().OnSelect?.Invoke();
    }

    private void UpFuction()
    {
        if(objectDown.GetComponent<Paper>() != null)
        {
            objectDown.GetComponent<Paper>().ReleasePaper();
        }

        objectRB = null;
        objectDown.GetComponent<Draggable>().holding = false;
        objectDown.GetComponent<Draggable>().GoBackward();
        objectDown = null;
    }

    public GameObject GetCurHolding()
    {
        return objectDown;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((pos - (Vector2)objectPos) + (Vector2)Camera.main.ScreenToWorldPoint(startMousePos), .1f);
    }
}
