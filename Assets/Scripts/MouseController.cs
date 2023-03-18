using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Vector2 minPaperSize;
    [SerializeField] private Vector2 maxPaperSize;

    private Vector2 mousePos;
    private Vector2 startMousePos;
    private Vector2 objectPos;
    private Vector2 pos;

    private bool hover = false;

    private GameObject objectDown;


    private void Update()
    {
        mousePos = Input.mousePresent ? Input.mousePosition : Vector2.zero;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Mouse click
        {
            startMousePos = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(mouse, Vector2.zero, 1f);
            if(hit.collider != null)
            {
                Draggable drag = hit.collider.GetComponent<Draggable>();
                if(drag != null)
                {
                    objectPos = drag.transform.position;
                    objectDown = drag.gameObject;
                    objectDown.GetComponent<Draggable>().GoForward();
                }
            }
        }

        Vector3 startMouse = Camera.main.ScreenToWorldPoint(startMousePos);

        if (Input.GetKey(KeyCode.Mouse0) && objectDown != null)
        {
            pos = mouse - startMouse + (Vector3)objectPos;
            objectDown.transform.position = pos;

            if(!GetIsInArea(minPaperSize, maxPaperSize, pos, objectDown.GetComponentInChildren<SpriteRenderer>().sprite))
            {
                UpFuction();
            }
            
        }

        if(Input.GetKeyUp(KeyCode.Mouse0) && objectDown != null) 
        {
            UpFuction();
        }
    }

    private void UpFuction()
    {
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
