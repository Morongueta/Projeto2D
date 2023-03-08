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

    public struct Vacancy
    {
        public string name;
        public int min;
        public int max;
    }

    public void ShowContracts()
    {
        ClearContracts();
        TextAsset txt = Resources.Load("NomesFemininos") as TextAsset;
        string[] feminineNames = txt.ToString().Split("\n");
        txt = Resources.Load("NomesMasculinos") as TextAsset;
        string[] masculineNames = txt.ToString().Split("\n");
        txt = Resources.Load("Vagas") as TextAsset;
        string[] vagas = txt.ToString().Split("\n");

        List<Vacancy> vacancy = new List<Vacancy>();

        for (int i = 0; i < vagas.Length; i++)
        {
            string[] vaga = vagas[i].Split(",");

            Vacancy v = new Vacancy();
            v.name = vaga[0];
            v.min = int.Parse(vaga[1]);
            v.max = int.Parse(vaga[2]);
            vacancy.Add(v);


        }

        string[] exp = new string[] { "Baixo", "Moderado", "Avançado" };

        curIndex = Mathf.Clamp(Mathf.RoundToInt(contracts.Count / 2), 0, contracts.Count - 1);
        contractObjects = new List<GameObject>();
        foreach (Contract cont in contracts)
        {
            float gender = (UnityEngine.Random.value * 100);
            GameObject obj = Instantiate(contractObject, contractPlacement);
            cont.ownerName = (gender < 50f) ? feminineNames[UnityEngine.Random.Range(0, feminineNames.Length)] : masculineNames[UnityEngine.Random.Range(0, masculineNames.Length)];
            cont.age = UnityEngine.Random.Range(14, 100);
            cont.cellphone = UnityEngine.Random.Range(0, 9999999);
            cont.gender = (gender < 50f) ? "Mulher" : "Homem" ;
            cont.civil = ((UnityEngine.Random.value * 100) < 50f) ? "Solteiro(a)" : "Casado(a)";
            cont.cellphone = UnityEngine.Random.Range(0, 9999999);

            Vacancy v = vacancy[UnityEngine.Random.Range(0, vacancy.Count)];

            int experience = UnityEngine.Random.Range(0, exp.Length);
            string salary = (100 * (int)Math.Round((UnityEngine.Random.Range(v.min, v.max) + ((experience + 1) * 550) - 550) / 100.0)).ToString();

            obj.GetComponent<Curriculum>().Set(cont.ownerName, cont.gender.ToString(), cont.age.ToString("D2"), cont.cellphone.ToString("D7"), cont.civil, "Vaga: " + v.name, exp[experience], "R$" + salary);
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
	    if(curIndex < 0) curIndex = contracts.Count - 1;
	    if(curIndex == contracts.Count) curIndex = 0;
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
}

[System.Serializable]
public class Contract
{
    //public Sprite face;
    public string ownerName;
    public int age;
    public int cellphone;

    public string gender;

    public string civil;

}
