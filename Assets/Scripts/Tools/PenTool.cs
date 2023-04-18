using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    private Vector2 mousePos;
    private Vector2 startMousePos;

    private Vector2 lastPos;

    private GameObject drawingOn;
    private LineRenderer curLine;
    private EdgeCollider2D edgeCol;

    private PenProperties pen;

    public void SetPen(PenProperties pen)
    {
        this.pen = pen;
    }
    public PenProperties GetPen()
    {
        return pen;
    }

    private void Update() 
    {
        if (ToolManager.i == null) return;
        if(ToolManager.i.GetTool() != Tool.PEN) return;

        mousePos = Input.mousePresent ? Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition) : Vector2.zero;


        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("VERDE0"))
        {
            startMousePos = Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition);
        }
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetButton("VERDE0"))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero, 1f);
            if(hit.Length == 0)
            {
                curLine = null;
                drawingOn = null;
                edgeCol = null;
                return;
            }
            RaycastHit2D closest = hit[0];
            int close = -9999999;
            for (int i = 0; i < hit.Length; i++)
            {
                Drawable paper = hit[i].collider.GetComponent<Drawable>();
                if(paper == null) return;

                if(paper.GetComponentInChildren<SpriteRenderer>().sortingOrder > close)
                {
                    closest = hit[i];
                    close = paper.GetComponentInChildren<SpriteRenderer>().sortingOrder;
                }
            }
            if(closest.collider != null)
            {
                Drawable draw = closest.collider.GetComponent<Drawable>();
                if(draw != null)
                {
                    if(drawingOn == draw.gameObject)
                    {
                        if(curLine != null)
                        {
                            if(Vector2.Distance(lastPos, mousePos) > .000001f)
                            {
                                curLine.positionCount++;
                                curLine.SetPosition(curLine.positionCount - 1, mousePos - (Vector2)drawingOn.transform.position);
                                lastPos = mousePos;
                                if(pen.isErasable)
                                {
                                    List<Vector2> p = new List<Vector2>();
                                    for (int i = 0; i < curLine.positionCount; i++)
                                    {
                                        p.Add(curLine.GetPosition(i));
                                    }
                                    edgeCol.SetPoints(p);
                                }else{
                                    edgeCol.enabled = false;
                                }
                            }
                        }
                    }else{
                        drawingOn = draw.gameObject;
                        GameObject line = Instantiate(pen.penObject, mousePos, Quaternion.identity);
                        line.transform.localScale = Vector3.one * .95f;
                        line.transform.parent = drawingOn.transform;
                        line.transform.localPosition = Vector2.zero;
                        lastPos = mousePos;
                        curLine = line.GetComponent<LineRenderer>();
                        edgeCol = line.GetComponent<EdgeCollider2D>();
                        curLine.sortingOrder = drawingOn.GetComponentInChildren<SpriteRenderer>().sortingOrder;
                        curLine.positionCount = 0;
                        curLine.positionCount++;
                        curLine.SetPosition(0, mousePos - (Vector2)drawingOn.transform.position);
                        if(pen.isErasable)
                        {
                            List<Vector2> p = new List<Vector2>();
                            for (int i = 0; i < curLine.positionCount; i++)
                            {
                                p.Add(curLine.GetPosition(i));
                            }
                            edgeCol.SetPoints(p);
                        }else{
                            edgeCol.enabled = false;
                        }
                    }
                }else
                {

                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetButtonUp("VERDE0"))
        {
            if(curLine != null)drawingOn.GetComponent<Drawable>().AddLineToList(curLine);
            if(curLine != null)if(curLine.positionCount < 2)
            {
                drawingOn.GetComponent<Drawable>().RemoveLineFromList(curLine);
                Destroy(curLine.gameObject);
            }
            curLine = null;
            drawingOn = null;
            edgeCol = null;
        }
    }
}
