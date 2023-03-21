using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tool
{
    HAND = 0,
    PEN = 1,
    ERASER = 2
}
public class ToolManager : MonoBehaviour
{
    [SerializeField]private Tool curTool;
    [SerializeField]private LayerMask toolLayer;

    public bool UsingHand, UsingTool;

    private int curIndex = 0;

    private ToolButton lastTool;

    public static ToolManager i;

    public Tool GetTool()
    {
        return curTool;
    }

    private void Awake()
    {
        i = this;
    }

    private void Update()
    {
        if(!UsingHand)
        {
            UsingTool = Input.GetKey(KeyCode.Mouse0);
        }


        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.CircleCast(pos, .25f, Vector2.zero, .25f, toolLayer);

            if(hit.collider != null)
            {
                ToolButton btn = hit.collider.GetComponent<ToolButton>();
                if(btn != null)
                {
                    if(curTool == btn.tool)
                    {
                        btn.SetColor(Color.white);
                        curTool = Tool.HAND;
                    }else{
                        if(lastTool != null) lastTool.SetColor(Color.white);
                        btn.SetColor(new Color(1f,1f,1f,.5f));
                        curTool = btn.tool;
                    }

                    lastTool = btn;
                }
            }
        }
    }
}
