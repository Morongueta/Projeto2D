using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HireState
{
    NONE,
    SELECTING,
    INTERVIEW
}

public class HiringManager : MonoBehaviour
{
    [SerializeField] private PaperBox negateBox;
    [SerializeField] private WorldButton selectionHiringButton;
    [SerializeField] private Vector2 visibleBoxPos;
    [SerializeField] private Vector2 hiddenBoxPos;
    [SerializeField] private Vector2 buttonVisiblePos;
    [SerializeField] private Vector2 buttonHiddenPos;
    [SerializeField] private HireState hireState;

    [Header("Hire - Interview")]
    private bool setupInterview;
    private bool startedInterview;

    private GameObject[] hirePapers;

    private Vector2 selectPos;

    private void Start()
    {
        selectionHiringButton.OnClickAction += () => 
        {
            FinishSelections();
        };
    }



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            hirePapers = PaperManager.i.GetHiringPapers(6);
            hireState = HireState.SELECTING;
            ShowBox(negateBox);
        }
        
        switch (hireState)
        {
            case HireState.SELECTING:
                SelectionPhase();
            break;
            case HireState.INTERVIEW:
                InterviewPhase();
            break;
        }
    }
    private void SelectionPhase()
    {
        if(negateBox.GetIsFull() && hireState == HireState.SELECTING)
        {
            if (selectPos != buttonVisiblePos)
            {
                selectPos = buttonVisiblePos;
                LeanTween.cancel(selectionHiringButton.gameObject);
                selectionHiringButton.gameObject.LeanMove(buttonVisiblePos, .25f);
            }
        }
        else
        {
            if (selectPos != buttonHiddenPos)
            {
                selectPos = buttonHiddenPos;
                LeanTween.cancel(selectionHiringButton.gameObject);
                selectionHiringButton.gameObject.LeanMove(buttonHiddenPos, .25f);
            }
        }
    }
    private void FinishSelections()
    {
        HideBox(negateBox);
        LeanTween.cancel(selectionHiringButton.gameObject);
        setupInterview = false;
        startedInterview = false;
        negateBox.DestroyFromBoxAll();
        
        selectionHiringButton.gameObject.LeanMove(buttonHiddenPos, .25f);
    }
    private void InterviewPhase()
    {
        if(setupInterview == false)
        {
            List<GameObject> papersRef = new List<GameObject>();
            Paper[] papers = FindObjectsOfType<Paper>();
            for (int i = 0; i < papers.Length; i++)
            {
                if (papers[i] != null && papers[i].type == PaperType.HIRE) papersRef.Add(papers[i].gameObject);
            }
            hirePapers = papersRef.ToArray();
            setupInterview = true;
            return;
        }
        
        if(startedInterview == false)
        {
            Debug.Log(hirePapers.Length);
            QueueManager.i.AddHiringPerson(hirePapers, true,1.5f);
            startedInterview = true;
            return;
        }
        
    }
    private void ShowBox(params PaperBox[] box)
    {
        for (int i = 0; i < box.Length; i++)
        {
            if(box[i].drag.enabled == false)
            {
                Draggable drag = box[i].drag;
                LeanTween.cancel(box[i].gameObject);
                box[i].gameObject.transform.LeanMove(visibleBoxPos, .35f).setOnComplete(()=>{drag.enabled = true;});
            }
        }
    }
    private void HideBox(params PaperBox[] box)
    {
        for (int i = 0; i < box.Length; i++)
        {
            if(box[i].drag.enabled == true)
            {
                box[i].drag.enabled = false;
                LeanTween.cancel(box[i].gameObject);
                box[i].gameObject.transform.LeanMove(hiddenBoxPos, .35f).setOnComplete(()=>hireState = HireState.INTERVIEW);
            }
        }
    }

    private void Clean(GameObject[] c)
    {
        List<GameObject> reference = new List<GameObject>();
        for (int i = 0; i < c.Length; i++)
        {
            if(c[i] != null)reference.Add(c[i]);
        }
        c = reference.ToArray();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hiddenBoxPos, .15f);
        Gizmos.DrawWireSphere(buttonHiddenPos, .15f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(visibleBoxPos, .15f);
        Gizmos.DrawWireSphere(buttonVisiblePos, .15f);
    }
}
