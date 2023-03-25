using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaperBox : MonoBehaviour
{
    [HideInInspector]public Draggable drag;
    [HideInInspector]public Vector2 startPos;
    [SerializeField] private int maxPapers;
    [SerializeField] private TextMeshPro countText;
    [SerializeField] private Vector2[] paperInsidePos;
    [SerializeField] private PaperType acceptPaperOfType;
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

    private void Update()
    {
        if (countText != null) countText.text = paperInside.Count + "/" + maxPapers;
    }

    private void UpdatePapers()
    {
        if (paperInside.Count <= 0) return;

        for (int i = 0; i < paperInside.Count; i++)
        {
            paperInside[i].transform.position = paperInsidePos[i] + (Vector2)transform.position; 
        }
    }

    public void AddToBox(GameObject add)
    {
        if (GetPaperCount() >= maxPapers) return;
        if(add.GetComponent<Paper>().type != acceptPaperOfType) return;
        Draggable drag = add.GetComponent<Draggable>();
        drag.active = false;
        drag.SetLayer(paperInside.Count + 1);
        add.GetComponent<Paper>().LockGravity(true);
        add.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        paperInside.Add(add);

        add.transform.parent = this.transform;
        add.transform.position = Vector3.zero;

        UpdatePapers();
    }

    public void RemoveFromBox()
    {
        if(paperInside.Count <= 0) return;

        GameObject paper = paperInside[paperInside.Count - 1];

        paper.GetComponent<Paper>().LockGravity(false);

        paper.GetComponent<Draggable>().active = true;
        paperInside.Remove(paper);
        paper.transform.parent = null;

        HandTool.i.TransferObject(paper);
    }

    public void RemoveFromBoxAll()
    {
        int count = paperInside.Count;
        while(count > 0)
        {
            GameObject paper = paperInside[paperInside.Count - 1];
            paper.GetComponent<Draggable>().active = true;
            paper.GetComponent<Paper>().ResetSize();
            paperInside.Remove(paper);
            paper.transform.parent = null;
            count--;
        }
    }

    public void DestroyFromBoxAll()
    {
        for (int i = 0; i < paperInside.Count; i++)
        {
            GameObject paper = paperInside[i];
            Destroy(paper);
        }
        paperInside.Clear();
    }

    public int GetPaperCount()
    {
        return paperInside.Count;
    }

    public bool GetIsFull()
    {
        return paperInside.Count >= maxPapers;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < paperInsidePos.Length; i++)
        {
            Gizmos.DrawWireSphere(paperInsidePos[i] + (Vector2)transform.position, .25f);
        }
    }
}
