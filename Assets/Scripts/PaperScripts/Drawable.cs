using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawable : MonoBehaviour
{
    private List<LineRenderer> drawedLines = new List<LineRenderer>();
    void Update()
    {
        
    }

    public void AddLineToList(LineRenderer add)
    {
        if(!drawedLines.Contains(add)) drawedLines.Add(add);
    }

    public void RemoveLineFromList(LineRenderer rem)
    {
        if(drawedLines.Contains(rem)) drawedLines.Add(rem);
    }

    public LineRenderer[] GetLines()
    {
        return drawedLines.ToArray();
    }
}
