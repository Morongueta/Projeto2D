using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DrawerState
{
    CHECK,
    COPY
}

[System.Serializable]
public struct DrawerButton
{
    public Button btn;
    public DrawerState state;
}

public class CoexistenceManager : MonoBehaviour
{
    [Header("Drawer")]
    [SerializeField]private AudioClip drawerClip;
    [SerializeField]private GameObject drawerObject;
    [SerializeField]private RectTransform curriculumArea;
    [SerializeField]private GameObject curriculumUIObject;
    [SerializeField] private GameObject flagObj;
    [SerializeField] private DrawerState drawerState;

    [SerializeField] private DrawerButton[] drawerButtons;

    private List<GameObject> curriculumUIList = new List<GameObject>();

    private bool drawerIsOpened;

    [Header("Persons")]
    public List<CurriculumData> personInCompany;

    public static CoexistenceManager i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        ChangeDrawerState(0);
    }

    public void ChangeDrawerState(int v)
    {
        drawerState = (DrawerState)v;
        UpdateDrawer();

        for (int i = 0; i < drawerButtons.Length; i++)
        {
            drawerButtons[i].btn.interactable = (drawerButtons[i].state != drawerState);
        }
    }

    public void UpdateDrawer()
    {
        if (drawerIsOpened) OpenDrawer();
    }

    public void OpenDrawer()
    {
        SoundManager.Instance.PlaySound(drawerClip, 0.05f);
        for (int i = 0; i < curriculumUIList.Count; i++)
        {
            Destroy(curriculumUIList[i]);
        }
        curriculumUIList.Clear();


        for (int i = 0; i < personInCompany.Count; i++)
        {
            int storedIndex = i;

            GameObject cur = Instantiate(curriculumUIObject, curriculumArea);

            switch (drawerState)
            {
                case DrawerState.CHECK:
                    cur.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].personName;
                    cur.transform.Find("JobText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].vaga;
                    cur.transform.Find("SalaryText").GetComponent<TextMeshProUGUI>().text = "Stress: " + personInCompany[i].stress * 100f;
                break;

                case DrawerState.COPY:
                    cur.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].personName;
                    cur.transform.Find("JobText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].vaga;
                    cur.transform.Find("SalaryText").GetComponent<TextMeshProUGUI>().text = "R$" + personInCompany[i].salary;
                break;
            }


            Transform flagT = cur.transform.Find("FlagArea").transform;

            for (int j = 0;j < personInCompany[i].redFlags; j++)
            {
                Instantiate(flagObj, flagT);
            }

            string workState = "";

            if(drawerState == DrawerState.COPY)
            {
                cur.GetComponent<Button>().onClick.AddListener(()=>{
                    GameObject paper = PaperManager.i.AddPersonPaper(personInCompany[storedIndex]);
                    paper.GetComponent<Paper>().SetCopy();
                });
            }else{
                CurriculumData d = personInCompany[i];
                cur.GetComponent<Button>().onClick.AddListener(()=>{
                    d.daysAway++;
                    d.workState = WorkState.AWAY;

                    d.ChangeStress(-0.35f);

                    UpdateDrawer();
                });
            }


            switch (personInCompany[i].workState)
            {
                case WorkState.AWAY:
                    workState = "Faltou";
                    break;
                case WorkState.WORKING:
                    break;
            }

            if(personInCompany[i].daysAway > 0)
            {
                workState = "Dias fora: " + personInCompany[i].daysAway;
            }

            cur.transform.Find("WorkingStateText").GetComponent<TextMeshProUGUI>().text = workState;

            

            curriculumUIList.Add(cur);
        }


        float curriculumAreaHeight = 25f;

        int rowQtd = Mathf.CeilToInt((float)personInCompany.Count / 5f);
        
        for (int i = 0; i < rowQtd; i++)
        {
            curriculumAreaHeight += 225;
        }

        curriculumAreaHeight += 25;

        curriculumArea.sizeDelta = new Vector2(0f,curriculumAreaHeight);

        drawerIsOpened = true;
        drawerObject.SetActive(true);
    }

    public void CloseDrawer()
    {
        for (int i = 0; i < curriculumUIList.Count; i++)
        {
            Destroy(curriculumUIList[i]);
        }
        curriculumUIList.Clear();

        drawerIsOpened = false;
        drawerObject.SetActive(false);
    }

    #region Person

    public void RemoveTutorial()
    {
        if(personInCompany.Count > 5)
        {
            personInCompany.RemoveAt(5);
        }
    }

    public void RemovePerson(Curriculum cur)
    {
        CurriculumData data = cur.Convert();

        RemovePerson(data);
    }

    public void RemovePerson(CurriculumData cur)
    {
        bool found = false;
        for (int i = 0; i < personInCompany.Count; i++)
        {
            CurriculumData data = personInCompany[i];
            if(ComparePersons(data,cur))
            {
                personInCompany.RemoveAt(i);
                found = true;
                break;
            }
        }
        if(found) return;
        Debug.LogError("Essa pessoa não está na empresa: " + cur.personName);   
    }


    public void AddPerson(Curriculum cur)
    {
        CurriculumData data = cur.Convert();

        AddPerson(data);
    }

    public void AddPerson(CurriculumData cur)
    {
        personInCompany.Add(cur);

        if(EventController.i != null) EventController.i.UpdateValue();
    }


    public void AddPerson()
    {
        CurriculumData data = FindObjectOfType<InformationDatabase>().GeneratePerson().Convert();
        AddPerson(data);
    }

    public void AddPerson(int length)
    {
        if(length <= 0) length = 1;

        for (int i = 0; i < length; i++)
        {
            AddPerson();
        }
    }

    public bool ComparePersons(CurriculumData personA, CurriculumData personB)
    {
        if(personA.personName.GetHashCode() == personB.personName.GetHashCode() && personA.salary.GetHashCode() == personB.salary.GetHashCode())
        {
            return true;
        }
        return false;
    }

    public bool ComparePersons(CurriculumData[] group, CurriculumData personB)
    {
        for (int i = 0; i < group.Length; i++)
        {
            if(ComparePersons(group[i], personB))
            {
                return true;
            }
        }
        return false;
    }

    public CurriculumData[] GetPersons()
    {
        return personInCompany.ToArray();
    }

    public CurriculumData[] GetWorkingPersons()
    {
        List<CurriculumData> workingPeople = new List<CurriculumData>();

        for (int i = 0; i < personInCompany.Count; i++)
        {
            if (personInCompany[i].workState == WorkState.WORKING) workingPeople.Add(personInCompany[i]);
        }

        return workingPeople.ToArray();
    }

    public CurriculumData GetPerson(int index)
    {
        if(index >= personInCompany.Count || index < 0) return default(CurriculumData);
        return personInCompany[index];
    }

    public CurriculumData GetRandomPerson(params CurriculumData[] persons)
    {
        List<CurriculumData> datas = new List<CurriculumData>();

        CurriculumData[] personsWorking = GetWorkingPersons();

        datas.AddRange(persons);
        if(persons.Length == 0)
        {
            return personsWorking[Random.Range(0, personsWorking.Length)];
        }else{
            CurriculumData result = personsWorking[Random.Range(0, personsWorking.Length)];
            while(ComparePersons(persons, result))
            {
                result = personsWorking[Random.Range(0, personsWorking.Length)];
            }

            return result;
        }
    }

    public void AddTrait(int personID, int traitID)
    {
        personInCompany[personID].AddTrait(FindObjectOfType<InformationDatabase>().GetTrait(traitID));

        if(EventController.i != null) EventController.i.UpdateValue();
    }

    #endregion
}
