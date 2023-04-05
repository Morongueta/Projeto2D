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


    public void AddPerson(Curriculum cur)
    {
        CurriculumData data = cur.Convert();

        personInCompany.Add(data);
    }

    public void AddTrait(int personID, int traitID, TraitType type)
    {
        for (int i = 0; i < personInCompany.Count; i++)
        {
            if(i == personID)
            {
                Trait[] traits = InformationDatabase.i.GetTraits(type);

                for (int y = 0; y < traits.Length; y++)
                {
                    if (traits[y].ID == traitID)
                    {
                        personInCompany[i].AddTrait(traits[y]);
                    }
                }
            }
            
        }
    }
}
