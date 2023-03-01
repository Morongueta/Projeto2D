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
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetCur(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetCur(1);
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
                curContract.transform.position = pos;
                //curContract.transform.localScale = Vector3.one * 1.25f;

                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = 1000;
            }
            else
            {
                //Debug.Log(());
                Vector3 offset = Vector3.zero;

                float offsetValue = 2.1f;
                if(Mathf.Abs(i - curIndex) >= 3)
                {
                    offsetValue *= 0.75f;
                }

                offset.x = offsetValue * (i - curIndex);
                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))) );
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))) );

                curContract.transform.position = contractPlacement.transform.position + offset;
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
