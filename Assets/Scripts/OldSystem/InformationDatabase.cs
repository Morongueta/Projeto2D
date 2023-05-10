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

    public Sprite[] masculineBodies;

    [Header("Feminine Parts")]
    public Sprite[] feminineBodies;
    public Sprite[] feminine_clothes;
    public Sprite[] feminine_heads;
    public Sprite[] feminine_mouths;
    public Sprite[] feminine_glasses;
    public Sprite[] feminine_noses;

    public static InformationDatabase i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        GetTexts();
    }

    private void GetTexts()
    {
        feminineNames = new string[0];
        masculineNames = new string[0];
        vacancyList = new List<Vacancy>();
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

    public PersonData GeneratePerson()
    {
        GetTexts();
        float gender = (UnityEngine.Random.value * 100);
        string ownerName = (gender < 50f) ? feminineNames[UnityEngine.Random.Range(0, feminineNames.Length)] : masculineNames[UnityEngine.Random.Range(0, masculineNames.Length)];
        int age = UnityEngine.Random.Range(14, 60);
        string cellphone = "("+ UnityEngine.Random.Range(10,99)+")" + UnityEngine.Random.Range(0, 9999999).ToString("D7");
        string genderN = (gender < 50f) ? "Mulher" : "Homem" ;
        string relr = (rel[UnityEngine.Random.Range(0, rel.Length)]);
        string civil = (gender < 50f) ? relr.Remove(relr.Length - 1, 1) + "a" : relr;

        Vacancy v = vacancyList[UnityEngine.Random.Range(0, vacancyList.Count)];

        int experience = UnityEngine.Random.Range(0, exp.Length);
        string salary = (100 * (int)Mathf.Round((UnityEngine.Random.Range(v.min, v.max) + ((experience + 1) * 550) - 550) / 100.0f)).ToString();
        v.variance = v.variance + (.1f * (experience + 1));
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

        //Apparence Generator
        int height = Random.Range(-30,90);
        int headType = (gender < 50f) ? Random.Range(0, feminine_heads.Length) : Random.Range(0, masculineBodies.Length);
        int clotheType = (gender < 50f) ? Random.Range(0, feminine_clothes.Length) : Random.Range(0, masculineBodies.Length);
        int noseType = (gender < 50f) ? Random.Range(0, feminine_noses.Length) : Random.Range(0, masculineBodies.Length);
        int mouthType = (gender < 50f) ? Random.Range(0, feminine_mouths.Length) : Random.Range(0, masculineBodies.Length);
        int glassesType = (gender < 50f) ? Random.Range(0, feminine_glasses.Length) : Random.Range(0, masculineBodies.Length);

        PersonData data = new PersonData(ownerName, genderN, vaga,v, cellphone, age.ToString("D2"), relr, civil, salary, exp[experience],0,0,headType,mouthType,glassesType,noseType,clotheType,height);
        data.c.positiveTraits = new Trait[0];
        data.c.negativeTraits = new Trait[0];
        return data;
    }

    public Trait[] GetRandomTraits(int length, TraitType traitType, CurriculumData curriculum)
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


    public Trait[] GetRandomTraits(int length, TraitType traitType, Trait[] lastTraits)
    {
        List<Trait> result = new List<Trait>();

        List<int> recordTraits = new List<int>();
        for (int i = 0; i < lastTraits.Length; i++)
        {
            recordTraits.Add(lastTraits[i].ID);
        }

        while (result.Count < length)
        {
            int randomTrait = Random.Range(0, traits.Length);
            if (!result.Contains(traits[randomTrait]) && traits[randomTrait].type == traitType && !recordTraits.Contains(traits[randomTrait].ID)) result.Add(traits[randomTrait]);
        }

        return result.ToArray();
    }


    public Trait[] GetTraits(TraitType traitType)
    {
        List<Trait> result = new List<Trait>();

        for (int i = 0; i < traits.Length; i++)
        {
            if (traits[i].type == traitType) result.Add(traits[i]);
        }

        return result.ToArray();
    }

    public Trait[] GetTraits()
    {
        return traits;
    }

    public Trait GetTrait(int index)
    {
        if(index >= traits.Length || index < 0) return null;
        return traits[index];
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

    [Range(-1f, 1f)] public float awayChance = 0f;
    [Range(-5,5)] public int contributionTime = 0;

    [Header("Responses")]
    public string[] traitDetail;
    public string[] contributionExplanation;

       
}

public struct PersonData
{
    public CurriculumData c;


    public PersonData(string name,string gender, string vaga, InformationDatabase.Vacancy v, string cellphone, string age, string relationship, string civil, string salary, string experience, int bodyType, int hairType, int headType, int mouthType, int glassesType, int noseType, int clothesType, int height)
    {
        c = new CurriculumData();

        c.personName = name;
        c.vaga = vaga;
        c.vacancy = v;
        c.gender = gender;
        c.cell = cellphone;
        c.age = age;
        c.civil = civil;
        c.salary = salary;
        c.exp = experience;

        c.height = height;

        c.bodyType = bodyType;
        c.hairType = hairType;
        c.headType = headType;
        c.glassesType = glassesType;
        c.mouthType = mouthType;
        c.noseType = noseType;
        c.headType = headType;
        c.clothesType = clothesType;
    }

    public CurriculumData Convert()
    {
        return c;
    }
}
