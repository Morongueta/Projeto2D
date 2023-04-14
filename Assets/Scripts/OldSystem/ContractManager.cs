using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public enum ContractState
{
    CHOOSE,
    INTERVIEW
}

public class ContractManager : MonoBehaviour
{
    [SerializeField] private float spaceBtwCurriculum;
    [SerializeField] private GameObject contractObject;
    [SerializeField] private Transform contractPlacement, interviewContainer;
    [SerializeField] private List<Contract> contracts;

    private ContractState curState;


    private List<int> selected = new List<int>();

    private int maxSelected = 4;

    private List<GameObject> contractObjects;

    private float timer;
    private int curIndex;

    private bool canInteract = false;

    

    public void ShowContracts()
    {
        ClearContracts();

        curState = ContractState.CHOOSE;

        curIndex = Mathf.Clamp(Mathf.RoundToInt(contracts.Count / 2), 0, contracts.Count - 1);
        contractObjects = new List<GameObject>();
        foreach (Contract cont in contracts)
        {
            GameObject obj = Instantiate(contractObject, contractPlacement.transform.position + Vector3.down * 10f, Quaternion.identity);
            obj.transform.parent = contractPlacement.transform;
            
            
            float gender = (UnityEngine.Random.value * 100);
            cont.ownerName = (gender < 50f) ? InformationDatabase.i.feminineNames[UnityEngine.Random.Range(0, InformationDatabase.i.feminineNames.Length)] : InformationDatabase.i.masculineNames[UnityEngine.Random.Range(0, InformationDatabase.i.masculineNames.Length)];
            cont.age = UnityEngine.Random.Range(14, 60);
            cont.cellphone = "("+ UnityEngine.Random.Range(10,99)+")" + UnityEngine.Random.Range(0, 9999999).ToString("D7");
            cont.gender = (gender < 50f) ? "Mulher" : "Homem" ;
            string relr = (InformationDatabase.i.rel[UnityEngine.Random.Range(0, InformationDatabase.i.rel.Length)]);
            cont.civil = (gender < 50f) ? relr.Remove(relr.Length - 1, 1) + "a" : relr;

            InformationDatabase.Vacancy v = InformationDatabase.i.vacancyList[UnityEngine.Random.Range(0, InformationDatabase.i.vacancyList.Count)];

            

            int experience = UnityEngine.Random.Range(0, InformationDatabase.i.exp.Length);
            string salary = (100 * (int)Math.Round((UnityEngine.Random.Range(v.min, v.max) + ((experience + 1) * 550) - 550) / 100.0f)).ToString();

            string removed = v.name.Replace("(a)", "");

            string femaleSimple = removed + "a";
            string femaleComplex = removed.Remove(removed.Length - 1, 1) + "a";

            string correct = removed;
            if(v.name.Contains("(a)") && gender < 50f)
            {
                if(removed[removed.Length - 1] == 'o')
                {
                    correct = femaleComplex;
                }else{
                    correct = femaleSimple;
                }   
            }

            string vaga = correct;

            obj.GetComponent<Curriculum>().Generate(cont.ownerName, cont.gender.ToString(), cont.age.ToString("D2"), cont.cellphone, cont.civil, "Vaga: " + vaga, InformationDatabase.i.exp[experience], "R$" + salary);
            contractObjects.Add(obj);
        }

        float timeToShow = .33f;

        for (int i = 0; i < contractObjects.Count; i++)
        {
            contractObjects[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));

            TextMeshPro[] texts = contractObjects[i].GetComponentsInChildren<TextMeshPro>();

            foreach (var text in texts)
            {
                text.sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));
            }


            if (i == contractObjects.Count - 1)
            {
                contractObjects[i].LeanMove(contractPlacement.transform.position, timeToShow).setOnComplete(() => { UpdateContracts(); canInteract = true; });
            }
            else if(i == curIndex)
            {
                contractObjects[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
                contractObjects[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;

                texts = contractObjects[i].GetComponentsInChildren<TextMeshPro>();

                foreach (var text in texts)
                {
                    text.sortingOrder = 1000;
                }

                contractObjects[i].LeanMove(contractPlacement.transform.position, timeToShow);
            }
            else
            {
                contractObjects[i].LeanMove(contractPlacement.transform.position, timeToShow);
            }
        }
        
        
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
        switch (curState)
        {
            case ContractState.CHOOSE:
                ChooseArea();
                break;
            case ContractState.INTERVIEW:
                InterviewArea();
                break;
        }
    }

    private void ChooseArea()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selected.Count >= maxSelected)
            {
                HideContracts();
                curState = ContractState.INTERVIEW;

                
            }
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SetCur(-1, contracts.Count, ()=> UpdateContracts());
            timer = .15f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SetCur(1, contracts.Count, () => UpdateContracts());
            timer = .15f;
        }
    }

    private void InterviewArea()
    {
        

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SetCur(-1, contractObjects.Count, () => UpdateInterview());
            timer = .15f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SetCur(1, contractObjects.Count, () => UpdateInterview());
            timer = .15f;
        }
        Debug.Log("Interview");
    }

    public void SetCur(int value, int count, Action onUpdate)
    {
        curIndex += value;
	    if(curIndex < 0) curIndex = count - 1;
	    if(curIndex == count) curIndex = 0;
        curIndex = Mathf.Clamp(curIndex, 0, count - 1);
        
        onUpdate?.Invoke();
    }

    public void HideContracts()
    {
        float timeToHide = .25f;
        canInteract = false;
        for (int i = 0; i < contractObjects.Count; i++)
        {
            GameObject cur = contractObjects[i];
            if(!selected.Contains(i))
            {
                cur.LeanMoveY(cur.transform.position.y - 10f, timeToHide).setOnComplete(() => { 
                    Destroy(cur.gameObject);

                    
                });
            }
            else
            {
                 //cur.LeanMove(contractPlacement.transform.position, timeToHide);
            }
        }

        


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
                curContract.GetComponentInChildren<SpriteRenderer>().color = Color.white;

                TextMeshPro[] texts = curContract.GetComponentsInChildren<TextMeshPro>();

                foreach (var text in texts)
                {
                    text.sortingOrder = 1000;
                }
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

                float grayScale = 1 - (.05f * Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));
                curContract.GetComponentInChildren<SpriteRenderer>().gameObject.LeanColor(new Color(grayScale, grayScale, grayScale, 1f),.05f);

                TextMeshPro[] texts = curContract.GetComponentsInChildren<TextMeshPro>();

                foreach (var text in texts)
                {
                    text.sortingOrder = (-Mathf.Abs(Mathf.RoundToInt(1 - (curIndex - i))));
                }

                curContract.LeanMove(contractPlacement.transform.position + offset, 0.05f);
                //curContract.transform.localScale = Vector3.one * (1 - (.15f * Mathf.Abs(i - curIndex)));
            }

        }
    }

    
    public void UpdateInterview()
    {
        
        for (int i = 0; i < contractObjects.Count; i++)
        {
            GameObject curContract = contractObjects[i];
            if (i == curIndex)
            {
                Vector3 pos = contractPlacement.transform.position;

                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
                curContract.GetComponentInChildren<SpriteRenderer>().color = Color.white;

                TextMeshPro[] texts = curContract.GetComponentsInChildren<TextMeshPro>();

                foreach (var text in texts)
                {
                    text.sortingOrder = 1000;
                }
            }
            else
            {
                curContract.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;

                TextMeshPro[] texts = curContract.GetComponentsInChildren<TextMeshPro>();

                foreach (var text in texts)
                {
                    text.sortingOrder = -2;
                }
            }

        }
    }
}

[System.Serializable]
public class Contract
{
    //public Sprite face;
    public string ownerName;
    public int age;
    public string cellphone;

    public string gender;

    public string civil;

}
