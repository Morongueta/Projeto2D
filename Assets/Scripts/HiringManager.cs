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
    [SerializeField]private PaperBox confirmBox;
    [SerializeField]private PaperBox negateBox;

    [SerializeField]private Vector2 visibleBoxPos;
    [SerializeField]private Vector2 hiddenBoxPos;

    [SerializeField]private HireState hireState;

    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            hireState = HireState.SELECTING;
            ShowBox(confirmBox,negateBox);
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
        if(FindObjectsOfType<Paper>().Length == 0 && hireState == HireState.SELECTING)
        {
            hireState = HireState.INTERVIEW;
            HideBox(negateBox,confirmBox);
        }
    }

    private void InterviewPhase()
    {
        //Devolver os papéis do confirmBox

        confirmBox.RemoveFromBoxAll();
    }
    private void ShowBox(params PaperBox[] box)
    {
        for (int i = 0; i < box.Length; i++)
        {
            if(box[i].drag.enabled == false)
            {
                Draggable drag = box[i].drag;
                LeanTween.cancel(box[i].gameObject);
                box[i].gameObject.transform.LeanMove(new Vector2(visibleBoxPos.x, box[i].startPos.y), .35f).setOnComplete(()=>{drag.enabled = true;});
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
                box[i].gameObject.transform.LeanMove(new Vector2(hiddenBoxPos.x, box[i].startPos.y), .35f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hiddenBoxPos, .15f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(visibleBoxPos, .15f);
    }
}
