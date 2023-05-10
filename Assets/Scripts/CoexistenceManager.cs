using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoexistenceManager : MonoBehaviour
{
    [Header("Drawer")]
    [SerializeField]private GameObject drawerObject;
    [SerializeField]private Transform curriculumArea;
    [SerializeField]private GameObject curriculumUIObject;

    private List<GameObject> curriculumUIList = new List<GameObject>();

    private bool drawerIsOpened;

    [Header("Persons")]
    public List<CurriculumData> personInCompany;

    public static CoexistenceManager i;

    private void Awake()
    {
        i = this;
    }

    public void UpdateDrawer()
    {
        if (drawerIsOpened) OpenDrawer();
    }

    public void OpenDrawer()
    {
        for (int i = 0; i < curriculumUIList.Count; i++)
        {
            Destroy(curriculumUIList[i]);
        }
        curriculumUIList.Clear();


        for (int i = 0; i < personInCompany.Count; i++)
        {
            int storedIndex = i;

            GameObject cur = Instantiate(curriculumUIObject, curriculumArea);

            cur.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].personName;
            cur.transform.Find("JobText").GetComponent<TextMeshProUGUI>().text = personInCompany[i].vaga;
            cur.transform.Find("SalaryText").GetComponent<TextMeshProUGUI>().text = "R$" + personInCompany[i].salary;

            string workState = "";

            switch (personInCompany[i].workState)
            {
                case WorkState.AWAY:
                    workState = "Faltou";
                    break;
                case WorkState.WORKING:
                    break;
            }

            cur.transform.Find("WorkingStateText").GetComponent<TextMeshProUGUI>().text = workState;


            cur.GetComponent<Button>().onClick.AddListener(()=>{
                GameObject paper = PaperManager.i.AddPersonPaper(personInCompany[storedIndex]);
                paper.GetComponent<Paper>().SetCopy();
            });

            curriculumUIList.Add(cur);
        }

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
        datas.AddRange(persons);
        if(persons.Length == 0)
        {
            return personInCompany[Random.Range(0, personInCompany.Count)];
        }else{
            CurriculumData result = personInCompany[Random.Range(0, personInCompany.Count)];
            while(ComparePersons(persons, result))
            {
                result = personInCompany[Random.Range(0, personInCompany.Count)];
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
