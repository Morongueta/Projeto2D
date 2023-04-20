using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    [SerializeField]private GameObject paperBase, paperCurriculum, paperContract;
    [SerializeField]private Vector3 paperSpawnPos;

    public static PaperManager i;

    private void Awake()
    {
        i = this;
    }

    public void AddPaper()
    {
        GameObject paper = Instantiate(paperBase, paperSpawnPos + new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)), Quaternion.identity);
        paper.GetComponent<Paper>().type = PaperType.NONE;
    }


    public void AddContractPaper(string text, Action confirm = null, Action decline = null)
    {
        GameObject paper = Instantiate(paperContract, paperSpawnPos + new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)), Quaternion.identity);
        paper.GetComponent<Paper>().type = PaperType.CONTRACT;

        Contract cont = paper.GetComponent<Contract>();

        cont.SetContractText(text);
        cont.confirmAction = confirm;
        cont.declineAction = decline;

    }

    public void AddPersonPaper(CurriculumData data)
    {
        GameObject paper = Instantiate(paperCurriculum, paperSpawnPos + new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)), Quaternion.identity);
        paper.GetComponent<Paper>().type = PaperType.NONE;

        paper.GetComponent<Curriculum>().Generate(data);
    }

    public GameObject[] GetHiringPapers(int paperQtd)
    {
        List<GameObject> hiringPapers = new List<GameObject>();

        for (int i = 0; i < paperQtd; i++)
        {
            GameObject paper = Instantiate(paperCurriculum, paperSpawnPos + new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)), Quaternion.identity);
            paper.GetComponent<Paper>().type = PaperType.HIRE;
            FillHirePaperRandomly(paper);
            hiringPapers.Add(paper);
        }

        return hiringPapers.ToArray();
    }

    private void FillHirePaperRandomly(GameObject paper)
    {
        PersonData data = InformationDatabase.i.GeneratePerson();
        paper.GetComponent<Curriculum>().Generate(data.Convert());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(paperSpawnPos, .15f);
    }
}
