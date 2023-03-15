using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationDatabase : MonoBehaviour
{
    [System.Serializable]
    public struct Vacancy
    {
        public string name;
        public int id;
        public int min;
        public int max;
        public float variance;
    }

    public List<Vacancy> vacancyList = new List<Vacancy>();

    public string[] masculineNames;
    public string[] feminineNames;

    public string[] exp = new string[] { "Baixo", "Moderado", "Avançado" };
    public string[] rel = new string[] { "Solteiro", "Casado", "Divorciado", "Viuvo" };

    public static InformationDatabase i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        TextAsset txt = Resources.Load("NomesFemininos") as TextAsset;
        feminineNames = txt.ToString().Replace("Srta. ", "").Replace("Sra. ", "").Replace("Dra. ", "").Split("\n");
        txt = Resources.Load("NomesMasculinos") as TextAsset;
        masculineNames = txt.ToString().Replace("Sr. ", "").Replace("Dr. ", "").Split("\n");
        txt = Resources.Load("Vagas") as TextAsset;
        string[] vagas = txt.ToString().Split("\n");

        for (int i = 0; i < vagas.Length; i++)
        {
            string[] vaga = vagas[i].Split(",");

            Vacancy v = new Vacancy();
            v.id = int.Parse(vaga[0]);
            v.name = vaga[1];
            v.min = int.Parse(vaga[2]);
            v.max = int.Parse(vaga[3]);
            v.variance = float.Parse(vaga[4]) / 100f;
            vacancyList.Add(v);
        }
    }
}
