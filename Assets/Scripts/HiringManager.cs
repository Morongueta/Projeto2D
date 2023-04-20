using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gatto.Utils;

public enum HireState
{
    NONE,
    SELECTING,
    INTERVIEW
}

public class HiringManager : MonoBehaviour
{
    public static HiringManager i;
    [SerializeField] private PaperBox negateBox;
    [SerializeField] private PaperBox confirmBox;
    [SerializeField] private WorldButton selectionHiringButton;
    [SerializeField] private WorldButton contractButton;
    [SerializeField] private Vector2 visibleBoxPos;
    [SerializeField] private Vector2 hiddenBoxPos;
    [SerializeField] private Vector2 buttonVisiblePos;
    [SerializeField] private Vector2 buttonHiddenPos;

    private int timerID = 939;
    private _PeriodTimer timer;
    public HireState hireState;

    [Header("Hire - Interview")]
    private bool setupInterview;
    private bool startedInterview;
    private GameObject[] hirePapers;
    private Vector2 selectPos;

    [Header("Interview")]
    private int maxInterviewMoney = 6;
    private int interviewMoney = 0;

    private void Awake()
    {
        i = this;    
    }

    private void Start()
    {
        contractButton.transform.position = buttonHiddenPos;
        selectionHiringButton.transform.position = buttonHiddenPos;

        interviewMoney = maxInterviewMoney;

        contractButton.OnClickAction += () => 
        {
            FinishInterview();
        };

        selectionHiringButton.OnClickAction += () => 
        {
            FinishSelections();
        };
    }

    public bool CanHire()
    {
        bool result = false;

        if(timer == null)
        {
            if(hireState == HireState.NONE) result = true;
        } 
        else result = false;

        return result;
    }

    public void StartHiring()
    {
        hirePapers = PaperManager.i.GetHiringPapers(6);
        hireState = HireState.SELECTING;
        ShowBox(negateBox);
    }

    private void Update()
    {

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
        if(AllPapersAreNull())
        {
            FinishSelections();
        }

        if(hireState == HireState.SELECTING)
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
        negateBox.DestroyFromBoxAll(.19f);
        
        selectionHiringButton.gameObject.LeanMove(buttonHiddenPos, .25f);
    }
    
    #region Interview Area
    private void InterviewPhase()
    {
        if(AllPapersAreNull())
        {
            FinishInterview();
        }

        if(setupInterview == false)
        {
            List<GameObject> papersRef = new List<GameObject>();
            Paper[] papers = FindObjectsOfType<Paper>();
            for (int i = 0; i < papers.Length; i++)
            {
                if (papers[i] != null && papers[i].type == PaperType.HIRE && papers[i].canGet == true) papersRef.Add(papers[i].gameObject);
            }
            hirePapers = papersRef.ToArray();
            setupInterview = true;
            return;
        }
        
        if(startedInterview == false)
        {
            Debug.Log(hirePapers.Length);
            QueueManager.i.AddHiringPerson(hirePapers, true);
            startedInterview = true;

            ShowBox(confirmBox);

            return;
        }

        if(!AllPapersAreNull())
        {
            if(confirmBox.GetIsFull() && hireState == HireState.INTERVIEW)
            {
                if (selectPos != buttonVisiblePos)
                {
                    selectPos = buttonVisiblePos;
                    LeanTween.cancel(contractButton.gameObject);
                    contractButton.gameObject.LeanMove(buttonVisiblePos, .25f);
                }
            }
            else
            {
                if (selectPos != buttonHiddenPos)
                {
                    selectPos = buttonHiddenPos;
                    LeanTween.cancel(contractButton.gameObject);
                    contractButton.gameObject.LeanMove(buttonHiddenPos, .25f);
                }
            }
        }else{
            FinishInterview();
        }
    }

    public bool AllPapersAreNull()
    {
        bool result = true;

        if(hirePapers.Length == 0) return true;

        for (int i = 0; i < hirePapers.Length; i++)
        {
            if(hirePapers[i] != null) result = false;
        }

        return result;
    }

    public void FinishInterview()
    {
        QueueManager.i.RemoveFromQueueInterview();
        HideBox(confirmBox);
        LeanTween.cancel(contractButton.gameObject);
        //negateBox.DestroyFromBoxAll();
        if(confirmBox.GetPapers().Length > 0)CoexistenceManager.i.AddPerson(confirmBox.GetPapers()[0].GetComponent<Curriculum>());
        hireState = HireState.NONE;
        confirmBox.DestroyFromBoxAll(.35f);
        contractButton.gameObject.LeanMove(buttonHiddenPos, .25f);
        
        timer = PeriodTimer.Timer(200f, null);
    }

    public void ResetInterviewMoney()
    {
        interviewMoney = maxInterviewMoney;
    }

    public int GetInverviewMoney()
    {
        return interviewMoney;
    }

    public void SpendInterviewMoney(int price)
    {
        interviewMoney -= price;
        if(interviewMoney < 0) interviewMoney = 0;
    }

    #endregion

    private void ShowBox(params PaperBox[] box)
    {
        for (int i = 0; i < box.Length; i++)
        {
            if(box[i].drag.enabled == false)
            {
                Draggable drag = box[i].drag;
                LeanTween.cancel(box[i].gameObject);
                Collider2D col = box[i].GetComponent<Collider2D>();
                
                box[i].gameObject.transform.LeanMove(visibleBoxPos, .35f).setOnComplete(()=>{
                    drag.enabled = true;
                    if(col != null) col.isTrigger = false;
                    });
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
                Collider2D col = box[i].GetComponent<Collider2D>();
                if(col != null) col.isTrigger = true;
                box[i].gameObject.transform.LeanMove(hiddenBoxPos, .35f).setOnComplete(()=>{
                    hireState = HireState.INTERVIEW;
                    });
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
