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

    public bool UsingHand, UsingTool;

    private int curIndex = 0;

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
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            curIndex++;
            if(curIndex > 2) curIndex = 0;
            curTool = (Tool)curIndex;
        }
    }
}
