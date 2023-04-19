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


    private void Update()
    {
        bool confirm = Physics2D.OverlapBox(confirmPosition + (Vector2)transform.position, Vector2.one * checkBoxSize, 0f, penLayer) != null;

        bool decline = Physics2D.OverlapBox(negatePosition + (Vector2)transform.position, Vector2.one * checkBoxSize, 0f, penLayer) != null;


        if(state == ContractState.NONE)
        {
		        Debug.Log("Waiting Input");
            if (confirm) { state = ContractState.CONFIRM; }

            if (decline) { state = ContractState.DECLINE; }
        }else if(state != ContractState.NONE)
        {
		        Debug.Log("Got Input");
            if(Input.GetButton("VERDE0") == false && Input.GetKey(KeyCode.Mouse0) == false)
            {
		        Debug.Log("Sent Input");
                GetComponent<Draggable>().active = false;
                transform.LeanMoveX(transform.position.x + 10f, 1f).setOnComplete(() =>
                {
                    Destroy(this.gameObject);
                });
                switch (state) { 
                
                    case ContractState.CONFIRM:
                        confirmAction?.Invoke();
                    break;
                    case ContractState.DECLINE:
                        declineAction?.Invoke();
                    break;

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
