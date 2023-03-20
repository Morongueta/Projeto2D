using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTool : MonoBehaviour
{
    [SerializeField]private LayerMask paperLayer;
    private Vector2 mousePos;
    private Vector2 cameraOffset;
    private Vector2 startMousePos;
    private Vector2 objectPos;
    private Vector2 pos;

    private Vector2 lastPos;

    private bool hover = false;

    private GameObject objectDown;

    private SpriteRenderer objectRender;
    private Rigidbody2D objectRB;


    private void Update()
    {
        if(ToolManager.i.GetTool() != Tool.HAND) return;

        mousePos = Input.mousePresent ? Input.mousePosition : Vector2.zero;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(mousePos);

        ToolManager.i.UsingHand = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Mouse click
        {
            startMousePos = Input.mousePosition;
            cameraOffset = Camera.main.transform.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll(mouse, Vector2.zero, 1f,paperLayer);
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
                }
            }
        }
        Vector3 startMouse = Camera.main.ScreenToWorldPoint(startMousePos);
        if (Input.GetKey(KeyCode.Mouse0) && objectDown != null)
        {
            pos = mouse - startMouse + (Vector3)objectPos;

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

    private void UpFuction()
    {
        // Rigidbody2D rb = objectRB;

        // Vector3 dir = (Vector2)Camera.main.ScreenToWorldPoint(mousePos) - pos;
        // Debug.Log(dir);
        // rb.AddForce(dir * 10f);


        
        objectRB = null;
        objectDown.GetComponent<Draggable>().holding = false;
        objectDown.GetComponent<Draggable>().GoBackward();
        objectDown = null;
    }

    public bool GetIsInArea(Vector2 min, Vector2 max, Vector2 pos, Sprite spr = null)
    {
        bool result = false;

        if(spr != null)
        {
            if((min.x < pos.x && max.x > pos.x) && (min.y < pos.y && max.y > pos.y))
            {
                result = true;

            }
        }
        else
        {
            if ((min.x < pos.x && max.x > pos.x) && (min.y < pos.y && max.y > pos.y))
            {
                result = true;

            }
        }

        return result;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((pos - (Vector2)objectPos) + (Vector2)Camera.main.ScreenToWorldPoint(startMousePos), .1f);
    }
}
