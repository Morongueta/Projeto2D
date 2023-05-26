using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum ContractState
{
    NONE,
    DECLINE,
    CONFIRM
}
public class Contract : MonoBehaviour
{
    private ContractState state = ContractState.NONE;
    [SerializeField] private Vector2 negatePosition;
    [SerializeField] private Vector2 confirmPosition;

    [SerializeField] private float checkBoxSize;

    [SerializeField] private LayerMask penLayer;
    [SerializeField] private TextMeshPro contractText;
    public Action confirmAction;
    public Action declineAction;

    private bool eventCalled;


    private void Update()
    {
        Collider2D[] confirmCollider = Physics2D.OverlapBoxAll(confirmPosition + (Vector2)transform.position, Vector2.one * checkBoxSize, 0f, penLayer);
        Collider2D[] declineCollider = Physics2D.OverlapBoxAll(negatePosition + (Vector2)transform.position, Vector2.one * checkBoxSize, 0f, penLayer);

        bool confirm = false;
        bool decline = false;

        for (int i = 0; i < confirmCollider.Length; i++)
        {
            if(confirmCollider[i].gameObject.transform.parent != null)
            {
                if(confirmCollider[i].gameObject.transform.parent == this.transform)
                {
                    confirm = true;
                    break;
                }
            }
        }
        for (int i = 0; i < declineCollider.Length; i++)
        {
            if(declineCollider[i].gameObject.transform.parent != null)
            {
                if(declineCollider[i].gameObject.transform.parent == this.transform)
                {
                    decline = true;
                    break;
                }
            }
        }

        if(state == ContractState.NONE)
        {
		        Debug.Log("Waiting Input");
            if (confirm) { state = ContractState.CONFIRM; }

            if (decline) { state = ContractState.DECLINE; }
        }else if(state != ContractState.NONE)
        {
		        Debug.Log("Got Input");
            if(CustomInput.GetKey(KeyCode.Mouse0, "VERDE0") == false)
            {
		        Debug.Log("Sent Input");
                GetComponent<Draggable>().active = false;
                transform.LeanMoveX(transform.position.x + 10f, 1f).setOnComplete(() =>
                {
                    Destroy(this.gameObject);
                });

                if(!eventCalled)
                {
                    switch (state) { 
                    
                        case ContractState.CONFIRM:
                            confirmAction?.Invoke();
                        break;
                        case ContractState.DECLINE:
                            declineAction?.Invoke();
                        break;
                    }  

                    eventCalled = true;
                }
            }
        }
    }

    public void SetContractText(string text)
    {
        contractText.text = text;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(negatePosition + (Vector2)transform.position, Vector2.one * checkBoxSize);
        Gizmos.DrawWireCube(confirmPosition + (Vector2)transform.position, Vector2.one * checkBoxSize);
    }
}
