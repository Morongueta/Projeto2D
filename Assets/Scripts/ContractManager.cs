using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ContractManager : MonoBehaviour
{
    [SerializeField] private float spaceBtwCurriculum;
    [SerializeField] private GameObject contractObject;
    [SerializeField] private Transform contractPlacement;
    [SerializeField] private List<Contract> contracts;

    private List<int> selected = new List<int>();

    private int maxSelected = 4;

    private List<GameObject> contractObjects;

    private float timer;
    private int curIndex;

    public void ShowContracts()
    {
        ClearContracts();

        curIndex = Mathf.Clamp(Mathf.RoundToInt(contracts.Count / 2), 0, contracts.Count - 1);
        contractObjects = new List<GameObject>();
        foreach (Contract cont in contracts)
        {
            GameObject obj = Instantiate(contractObject, contractPlacement);
            obj.GetComponentInChildren<TextMeshPro>().text = cont.ownerName;
            contractObjects.Add(obj);
        }
        UpdateContracts();
    }

    public void ClearContracts()
    {
        if (contractObjects != null)
        {
            foreach (GameObject obj in contractObjects)
            {
                Destroy(obj);
            }
        }
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selected.Contains(curIndex))
            {
                selected.Remove(curIndex);
                UpdateContracts();
                return;
            }

            if (selected.Count < maxSelected)
            {
                selected.Add(curIndex);
                UpdateContracts();
            }


        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SetCur(-1);
            timer = .15f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SetCur(1);
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
        for (int i = 0; i < contractObjects.Count; i++)
        {
            GameObject curContract = contractObjects[i];
            if (i == curIndex)
            {
                Vector3 pos = contractPlacement.transform.position;
                pos.y += (selected.Contains(i) ? .5f : .25f);

                curContract.LeanMove(pos, 0.05f);
                //curContract.transform.localScale = Vector3.one * 1.25f;

                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = 1000;
            }
            else
            {
                //Debug.Log(());
                Vector3 offset = Vector3.zero;

                float offsetValue = spaceBtwCurriculum;
                offsetValue *= Mathf.Max(3 - (.15f * Mathf.Abs(i - curIndex)), 0f);
                //Debug.Log(offsetValue);

                offset.x = offsetValue * (i - curIndex);
                offset.y = (selected.Contains(i) ? .5f : 0f);
                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));
                curContract.GetComponentInChildren<TextMeshPro>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));

                curContract.LeanMove(contractPlacement.transform.position + offset, 0.05f);
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
