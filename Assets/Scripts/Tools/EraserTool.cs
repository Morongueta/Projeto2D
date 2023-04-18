using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserTool : MonoBehaviour
{
    [SerializeField]private float eraserSize;
    [SerializeField]private LayerMask lineLayer, paperLayer;
    private Vector2 mousePos;
    private Vector2 startMousePos;

    private Drawable paperDraw;

    private void Update() 
    {
        if(ToolManager.i.GetTool() != Tool.ERASER) return;

        mousePos = Input.mousePresent ? Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition) : Vector2.zero;


        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("VERDE0"))
        {
            startMousePos = Camera.main.ScreenToWorldPoint(CustomMouse.i.mousePosition);
        }
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetButton("VERDE0"))
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(mousePos, eraserSize, Vector2.zero, eraserSize, lineLayer);
            RaycastHit2D[] hitPaper = Physics2D.RaycastAll(mousePos, Vector2.zero, 1f, paperLayer);
            if(hit.Length == 0) return;
            if(hitPaper.Length == 0) return;

            for (int i = 0; i < hit.Length; i++)
            {
                if(hit[i].collider != null)
                {
                    LineRenderer line = hit[i].collider.GetComponent<LineRenderer>();
                    if(line != null)
                    {
                        GameObject frontPaper = null;
                        int front = -1000000;
                        
                        for (int z = 0; z < hitPaper.Length; z++)
                        {
                            if(hitPaper[z].collider == null) return;
                            SpriteRenderer rend = hitPaper[z].collider.GetComponentInChildren<SpriteRenderer>();
                            if(rend == null) return;
                            if(rend.sortingOrder > front)
                            {
                                front = rend.sortingOrder;
                                frontPaper = hitPaper[z].collider.gameObject;
                            }
                            
                        }
                        if(frontPaper == null) return;
                        SpriteRenderer paper = frontPaper.GetComponentInChildren<SpriteRenderer>();
                        if(paper == null) return;
                        if(line.sortingOrder != paper.sortingOrder) return;

                        int closeIndex = -1;
                        float close = 10000000;
                        if(line.positionCount < 1)
                        {
                            Destroy(line.gameObject);
                            i = 0;
                            return;
                        }
                        for (int y = 0; y < line.positionCount; y++)
                        {
                            if(Vector2.Distance(line.GetPosition(y) + line.transform.position, mousePos) < close)
                            {
                                Debug.Log("Detected Close " + y);
                                close = Vector2.Distance(line.GetPosition(y) + line.transform.position, mousePos);
                                closeIndex = y;
                            }
                        }
                        if(closeIndex == -1) return;
                        Debug.Log("Splitting");
                        //Split Line Renderer

                        if(closeIndex == 0)
                        {
                            List<Vector3> poss = new List<Vector3>();
                            for (int x = 0; x < line.positionCount; x++)
                            {
                                if(x > closeIndex)
                                {
                                    poss.Add(line.GetPosition(x));
                                }
                            }
                            line.positionCount = poss.Count;
                            line.SetPositions(poss.ToArray());

                            List<Vector2> ps = new List<Vector2>();
                            for (int g = 0; g < line.positionCount; g++)
                            {
                                ps.Add(line.GetPosition(g));
                            }
                            line.GetComponent<EdgeCollider2D>().SetPoints(ps);
                            
                            if(line.positionCount < 1)
                            {
                                paperDraw = line.GetComponentInParent<Drawable>();
                                if(paperDraw != null) paperDraw.RemoveLineFromList(line);
                                Destroy(line.gameObject);
                                i = 0;
                                return;
                            }
                            
                            return;
                        }

                        if(closeIndex == line.positionCount - 1)
                        {
                            List<Vector3> poss = new List<Vector3>();
                            for (int x = 0; x < line.positionCount; x++)
                            {
                                if(x < closeIndex)
                                {
                                    poss.Add(line.GetPosition(x));
                                }
                            }
                            line.positionCount = poss.Count;
                            line.SetPositions(poss.ToArray());

                            List<Vector2> ps = new List<Vector2>();
                            for (int g = 0; g < line.positionCount; g++)
                            {
                                ps.Add(line.GetPosition(g));
                            }
                            line.GetComponent<EdgeCollider2D>().SetPoints(ps);

                            if(line.positionCount < 1)
                            {
                                paperDraw = line.GetComponentInParent<Drawable>();
                                if(paperDraw != null) paperDraw.RemoveLineFromList(line);
                                Destroy(line.gameObject);
                                i = 0;
                                return;
                            }

                            return;
                        }
                        
                        GameObject cloneLineObj = Instantiate(line.gameObject, line.gameObject.transform.parent);
                        LineRenderer cloneLine = cloneLineObj.GetComponent<LineRenderer>();
                        paperDraw = line.GetComponentInParent<Drawable>();
                        if(paperDraw != null) paperDraw.AddLineToList(cloneLine);

                        line.positionCount = closeIndex - 1;

                        Vector3[] linePos = new Vector3[cloneLine.positionCount];
                        cloneLine.GetPositions(linePos);
                        List<Vector3> pos = new List<Vector3>();

                        for (int x = 0; x < cloneLine.positionCount; x++)
                        {
                            if(x > line.positionCount)
                            {
                                pos.Add(cloneLine.GetPosition(x));
                            }
                        }
                        cloneLine.positionCount = pos.Count;
                        cloneLine.SetPositions(pos.ToArray());


                        List<Vector2> p = new List<Vector2>();
                        for (int g = 0; g < line.positionCount; g++)
                        {
                            p.Add(line.GetPosition(g));
                        }
                        line.GetComponent<EdgeCollider2D>().SetPoints(p);

                        p = new List<Vector2>();
                        for (int g = 0; g < cloneLine.positionCount; g++)
                        {
                            p.Add(cloneLine.GetPosition(g));
                        }
                        cloneLine.GetComponent<EdgeCollider2D>().SetPoints(p);

                        if(line.positionCount < 1)
                        {
                            if(paperDraw != null) paperDraw.RemoveLineFromList(line);
                            Destroy(line.gameObject);
                            i = 0;
                            return;
                        }

                        if(cloneLine.positionCount < 1)
                        {
                            if(paperDraw != null) paperDraw.RemoveLineFromList(cloneLine);
                            Destroy(cloneLine.gameObject);
                            i = 0;
                            return;
                        }
                    }
                }
            }
            
        }

    }

    private void OnDrawGizmos()
    {
        if(ToolManager.i != null)
        {
            if(ToolManager.i.GetTool() != Tool.ERASER) return;
            Gizmos.DrawWireSphere(mousePos, eraserSize);
        }
    }
}
