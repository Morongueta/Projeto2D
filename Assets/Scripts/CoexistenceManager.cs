using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoexistenceManager : MonoBehaviour
{
    
    [SerializeField]private List<CurriculumData> personInCompany;

    public static CoexistenceManager i;

    private void Awake()
    {
        i = this;
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
        if(index >= personInCompany.Count || index < 0) return null;
        return personInCompany[index];
    }

    public void AddTrait(int personID, int traitID)
    {
        personInCompany[personID].AddTrait(FindObjectOfType<InformationDatabase>().GetTrait(traitID));

        if(EventController.i != null) EventController.i.UpdateValue();
    }

    #endregion
}