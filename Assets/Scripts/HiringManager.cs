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
    [SerializeField]private PaperBox negateBox;

    [SerializeField] private WorldButton selectionHiringButton;

    [SerializeField]private Vector2 visibleBoxPos;
    [SerializeField]private Vector2 hiddenBoxPos;
    [SerializeField] private Vector2 buttonVisiblePos;
    [SerializeField]private Vector2 buttonHiddenPos;

    [SerializeField]private HireState hireState;

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
        hireState = HireState.INTERVIEW;
        HideBox(negateBox);
        LeanTween.cancel(selectionHiringButton.gameObject);
        selectionHiringButton.gameObject.LeanMove(buttonHiddenPos, .25f);
        negateBox.DestroyFromBoxAll();
    }

    private void InterviewPhase()
    {
        //Devolver os papéis do confirmBox

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
                box[i].gameObject.transform.LeanMove(hiddenBoxPos, .35f);
            }
        }
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
