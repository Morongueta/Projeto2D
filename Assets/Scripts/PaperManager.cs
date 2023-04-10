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
        PersonData data = InformationDatabase.i.GeneratePerson();
        paper.GetComponent<Curriculum>().Set(data.name, data.gender, data.age, data.cellphone, data.civil, "Vaga: " + data.vaga, data.experience, "R$" + data.salary);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(paperSpawnPos, .15f);
    }
}
