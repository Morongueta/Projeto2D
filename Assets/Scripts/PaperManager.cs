using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    [SerializeField]private GameObject paperBase;
    [SerializeField]private Vector3 paperSpawnPos;

    public static PaperManager i;

    private void Awake()
    {
        i = this;
    }

    public GameObject[] GetHiringPapers(int paperQtd)
    {
        List<GameObject> hiringPapers = new List<GameObject>();

        for (int i = 0; i < paperQtd; i++)
        {
            GameObject paper = Instantiate(paperBase, paperSpawnPos + new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f)), Quaternion.identity);
            paper.GetComponent<Paper>().type = PaperType.HIRE;
            FillHirePaperRandomly(paper);
            hiringPapers.Add(paper);
        }

        return hiringPapers.ToArray();
    }

    private void FillHirePaperRandomly(GameObject paper)
    {
        float gender = (UnityEngine.Random.value * 100);
        string ownerName = (gender < 50f) ? InformationDatabase.i.feminineNames[UnityEngine.Random.Range(0, InformationDatabase.i.feminineNames.Length)] : InformationDatabase.i.masculineNames[UnityEngine.Random.Range(0, InformationDatabase.i.masculineNames.Length)];
        int age = UnityEngine.Random.Range(14, 60);
        string cellphone = "("+ UnityEngine.Random.Range(10,99)+")" + UnityEngine.Random.Range(0, 9999999).ToString("D7");
        string genderN = (gender < 50f) ? "Mulher" : "Homem" ;
        string relr = (InformationDatabase.i.rel[UnityEngine.Random.Range(0, InformationDatabase.i.rel.Length)]);
        string civil = (gender < 50f) ? relr.Remove(relr.Length - 1, 1) + "a" : relr;

        InformationDatabase.Vacancy v = InformationDatabase.i.vacancyList[UnityEngine.Random.Range(0, InformationDatabase.i.vacancyList.Count)];

        

        int experience = UnityEngine.Random.Range(0, InformationDatabase.i.exp.Length);
        string salary = (100 * (int)Mathf.Round((UnityEngine.Random.Range(v.min, v.max) + ((experience + 1) * 550) - 550) / 100.0f)).ToString();

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

        paper.GetComponent<Curriculum>().Set(ownerName, genderN.ToString(), age.ToString("D2"), cellphone, civil, "Vaga: " + vaga, InformationDatabase.i.exp[experience], "R$" + salary);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(paperSpawnPos, .15f);
    }
}
