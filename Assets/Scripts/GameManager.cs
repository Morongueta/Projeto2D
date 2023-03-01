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
            obj.GetComponentInChildren<TextMeshProUGUI>().text = cont.ownerName;
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
                curContract.transform.position = contractPlacement.transform.position;
            }
            else
            {
                Vector3 offset = Vector3.zero;

                curContract.transform.position = contractPlacement.transform.position + offset;
            }
            
        }
    }
}

[System.Serializable]
public class Contract
{
    public string ownerName;
}
