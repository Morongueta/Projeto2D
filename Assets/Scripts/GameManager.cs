using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject contractObject;
    [SerializeField] private Transform contractPlacement;
    [SerializeField] private List<Contract> contracts;

    private List<GameObject> contractObjects;

    private float timer;
    private int curIndex;

    private void Start()
    {
        contractObjects = new List<GameObject>();   
        foreach(Contract cont in contracts)
        {
            GameObject obj = Instantiate(contractObject, contractPlacement);
            obj.GetComponentInChildren<TextMeshPro>().text = cont.ownerName;
            contractObjects.Add(obj);
        }
        UpdateContracts();
    }

    private void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            SetCur(1);
            timer = .15f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SetCur(-1);
            timer = .15f;
        }
    }

    public void SetCur(int value)
    {
        curIndex += value;
        curIndex = Mathf.Clamp(curIndex, 0, contracts.Count - 1);
        UpdateContracts();
    }

    public void UpdateContracts()
    {
        for(int i = 0; i < contractObjects.Count; i++)
        {
            GameObject curContract = contractObjects[i];
            if (i == curIndex)
            {
                Vector3 pos = contractPlacement.transform.position;
                
                curContract.LeanMove(pos,0.05f);
                //curContract.transform.localScale = Vector3.one * 1.25f;

                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = 1000;
            }
            else
            {
                //Debug.Log(());
                Vector3 offset = Vector3.zero;

                float offsetValue = 1f;
                offsetValue *= (2 - (.15f * Mathf.Abs(i - curIndex)));

                offset.x = offsetValue * (i - curIndex);
                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))) );
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))) );

                curContract.LeanMove(contractPlacement.transform.position + offset,0.05f);
                //curContract.transform.localScale = Vector3.one * (1 - (.15f * Mathf.Abs(i - curIndex)));
            }
            
        }
    }
}

[System.Serializable]
public class Contract
{
    public string ownerName;
}
