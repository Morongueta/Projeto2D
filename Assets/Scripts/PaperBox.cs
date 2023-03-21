using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBox : MonoBehaviour
{
    [HideInInspector]public Draggable drag;
    [HideInInspector]public Vector2 startPos;
    private List<GameObject> paperInside = new List<GameObject>();

    private void Awake()
    {
        startPos = transform.position;
        drag = GetComponent<Draggable>();
        drag.OnSelect += () => {
            RemoveFromBox();
        };
        drag.enabled = false;
    }

    public void AddToBox(GameObject add)
    {
        paperInside.Add(add);
        add.SetActive(false);
    }

    public void RemoveFromBox()
    {
        if(paperInside.Count <= 0) return;
        GameObject paper = paperInside[paperInside.Count - 1];
        paperInside.Remove(paper);

        HandTool.i.TransferObject(paper);
        paper.SetActive(true);
    }

    public void RemoveFromBoxAll()
    {
        int count = paperInside.Count;
        while(count > 0)
        {
            GameObject paper = paperInside[paperInside.Count - 1];
            paper.GetComponent<Draggable>().GoForward();
            paper.GetComponent<Paper>().ResetSize();
            paper.GetComponent<Draggable>().GoBackward();
            paperInside.Remove(paper);
            paper.SetActive(true);
            count--;
        }
    }
}
