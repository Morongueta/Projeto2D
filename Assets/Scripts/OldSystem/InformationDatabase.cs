using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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

    public string[] exp = new string[] { "Baixo", "Moderado", "Avan�ado" };
    public string[] rel = new string[] { "Solteiro", "Casado", "Divorciado", "Viuvo" };
    public Trait[] traits;

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

    public Trait[] GetRandomTraits(int length, TraitType traitType, Curriculum curriculum)
    {
        List<Trait> result = new List<Trait>();
        List<int> traitsID = new List<int>(); 
        Trait[] traitsGot = ((traitType == TraitType.POSITIVE) ? curriculum.negativeTraits : curriculum.positiveTraits);

        for (int i = 0; i < traitsGot.Length; i++)
        {
            traitsID.Add(traitsGot[i].ID);
        }


        while (result.Count < length)
        {
            int randomTrait = Random.Range(0, traits.Length);

            if(traitsID.Count > 0)
            {
                if(traits[randomTrait].BlackListID.Length > 0)
                {
                    //Checa por blacklist
                    for (int i = 0; i < traits[randomTrait].BlackListID.Length; i++)
                    {
                        if (!traitsID.Contains(traits[randomTrait].BlackListID[i]))
                        {
                            if (!result.Contains(traits[randomTrait]) && traits[randomTrait].type == traitType) result.Add(traits[randomTrait]);
                        }
                    }
                }
                else
                {
                    if (!result.Contains(traits[randomTrait]) && traits[randomTrait].type == traitType) result.Add(traits[randomTrait]);
                }
                
            }
            else
            {
                if (!result.Contains(traits[randomTrait]) && traits[randomTrait].type == traitType) result.Add(traits[randomTrait]);
            }
            
            
            
        }

        return result.ToArray();
    }
}

public enum TraitType
{
    POSITIVE,
    NEGATIVE
}


[System.Serializable]
public class Trait
{
    public string name;
    public int ID;
    public int[] BlackListID;
    public TraitType type;
    [Range(-1f, 1f), Tooltip("Esse atributo melhora ou piora o rendimento da pessoa")] public float performance = 0f;

    [Range(-1f, 1f), Tooltip("Esse atributo aumenta ou diminui a chance de eventos do tipo \"conflito\"")] public float agressiveness = 0f;

    [Range(-1f, 1f), Tooltip("Esse atributo aumenta ou diminui a chance de eventos do tipo \"coisas quebrando\"")] public float disastrous = 0f;

    [Range(-1f, 1f), Tooltip("Esse atributo aumenta ou diminui o entrosamento da equipe\nIsso altera o rendimento extra de cada mês juntamente altera a chance de conflitos")] public float responsability = 0f;

    [Range(-1f, 1f), Tooltip("Esse atributo aumenta ou diminui o tempo que aquela pessoa ficará na empresa")] public float stayFactor = 0f;

    [Range(0f, 1f)] public float awayChance = 0f;
       
}


