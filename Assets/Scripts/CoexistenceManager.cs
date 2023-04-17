using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoexistenceManager : MonoBehaviour
{
    [Header("Drawer")]
    [SerializeField]private GameObject drawerObject;
    [SerializeField]private Transform curriculumArea;
    [SerializeField]private GameObject curriculumUIObject;

    private List<GameObject> curriculumUIList = new List<GameObject>();

    [Header("Persons")]
    [SerializeField]private List<CurriculumData> personInCompany;

    public static CoexistenceManager i;

    private void Awake()
    {
        i = this;
    }

    public void OpenDrawer()
    {
        curriculumUIList = new List<GameObject>();
        for (int i = 0; i < personInCompany.Count; i++)
        {
            GameObject cur = Instantiate(curriculumUIObject, curriculumArea);

            curriculumUIList.Add(cur);
        }


        drawerObject.SetActive(true);
    }

    public void CloseDrawer()
    {
        for (int i = 0; i < curriculumUIList.Count; i++)
        {
            Destroy(curriculumUIList[i]);
        }
        curriculumUIList.Clear();

        drawerObject.SetActive(false);
    }

    #region Person

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

    public CurriculumData[] GetPersons()
    {
        return personInCompany.ToArray();
    }

    public CurriculumData GetPerson(int index)
    {
        if(index >= personInCompany.Count || index < 0) return default(CurriculumData);
        return personInCompany[index];
    }

    public CurriculumData GetRandomPerson()
    {
        return personInCompany[Random.Range(0, personInCompany.Count)];
    }

    public void AddTrait(int personID, int traitID)
    {
        personInCompany[personID].AddTrait(FindObjectOfType<InformationDatabase>().GetTrait(traitID));

        if(EventController.i != null) EventController.i.UpdateValue();
    }

    #endregion
}
