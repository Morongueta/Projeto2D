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
}
