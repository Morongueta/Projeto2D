using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Gatto.Utils;

public class Curriculum : MonoBehaviour
{
    public CurriculumData curriculumData = new CurriculumData();

    public void Generate(string name, string gender, string age, string cell, string civil, string vaga, InformationDatabase.Vacancy vacancy, string exp, string salary)
    {
        curriculumData = new CurriculumData();

        curriculumData.personName = name;
        curriculumData.gender = gender;
        curriculumData.age = age;
        curriculumData.cell = cell;
        curriculumData.civil = civil;
        curriculumData.vaga = vaga;
        curriculumData.vacancy = vacancy;
        curriculumData.exp = exp;
        curriculumData.salary = salary;

        curriculumData.hasFamily = ((Random.value * 100) < 50f);
        if(curriculumData.hasFamily) curriculumData.sonsQtd = Random.Range(1,5);

        if((Random.value * 100f) < 50f)
        {
            curriculumData.positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, curriculumData);
            curriculumData.negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, curriculumData);
        }
        else
        {
            curriculumData.negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, curriculumData);
            curriculumData.positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, curriculumData);
        }
        List<Trait> traits = new List<Trait>();
        traits.AddRange(curriculumData.negativeTraits);
        traits.AddRange(curriculumData.positiveTraits);
        for (int i = 0; i < traits.Count; i++)
        {
            curriculumData.contributionTime += traits[i].contributionTime;
        }

        curriculumData.contributionTime += Random.Range(5,10);

        curriculumData.height = Random.Range(50,90);

        if(GetComponent<CurriculumUI>() != null)
        {
            GetComponent<CurriculumUI>().Set(curriculumData);
        }
        
    }

    public void Generate(CurriculumData data)
    {
        curriculumData = new CurriculumData();

        curriculumData.personName = data.personName;
        curriculumData.gender = data.gender;
        curriculumData.age = data.age;
        curriculumData.cell = data.cell;
        curriculumData.civil = data.civil;
        curriculumData.vaga = data.vaga;
        curriculumData.vacancy = data.vacancy;
        curriculumData.exp = data.exp;
        curriculumData.salary = data.salary;

        curriculumData.hasFamily = ((Random.value * 100) < 50f);
        if(curriculumData.hasFamily) curriculumData.sonsQtd = Random.Range(1,5);

        curriculumData.positiveTraits = new Trait[0];
        curriculumData.negativeTraits = new Trait[0];

        if((Random.value * 100f) < 50f)
        {
            curriculumData.positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, curriculumData);
            curriculumData.negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, curriculumData);
        }
        else
        {
            curriculumData.negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, curriculumData);
            curriculumData.positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, curriculumData);
        }
        List<Trait> traits = new List<Trait>();
        traits.AddRange(curriculumData.negativeTraits);
        traits.AddRange(curriculumData.positiveTraits);
        for (int i = 0; i < traits.Count; i++)
        {
            curriculumData.contributionTime += traits[i].contributionTime;
        }

        curriculumData.contributionTime += Random.Range(5,10);

        curriculumData.height = Random.Range(50,90);

        curriculumData.headType = data.headType;
        curriculumData.hairType = data.hairType;
        curriculumData.bodyType = data.bodyType;
        curriculumData.glassesType = data.glassesType;
        curriculumData.clothesType = data.clothesType;
        curriculumData.mouthType = data.mouthType;
        curriculumData.noseType = data.noseType;

        curriculumData.skinHex = data.skinHex;
        curriculumData.hairHex = data.hairHex;
        curriculumData.clothesHex = data.clothesHex;

        if(GetComponent<CurriculumUI>() != null)
        {
            GetComponent<CurriculumUI>().Set(curriculumData);
        }
        
    }


    public void Set(CurriculumData data)
    {
        curriculumData = new CurriculumData();

        curriculumData.personName = data.personName;
        curriculumData.gender = data.gender;
        curriculumData.age = data.age;
        curriculumData.cell = data.cell;
        curriculumData.civil = data.civil;
        curriculumData.vaga = data.vaga;
        curriculumData.vacancy = data.vacancy;
        curriculumData.exp = data.exp;
        curriculumData.salary = data.salary;

        curriculumData.hasFamily = data.hasFamily;
        curriculumData.sonsQtd = data.sonsQtd;

        curriculumData.positiveTraits = data.positiveTraits;
        curriculumData.negativeTraits = data.negativeTraits;

        curriculumData.contributionTime = data.contributionTime;

        if(GetComponent<CurriculumUI>() != null)
        {
            GetComponent<CurriculumUI>().Set(curriculumData);
        }
    }

    public CurriculumData Convert()
    {
        CurriculumData c = new CurriculumData();

        c.Store(this);

        return c;
    }

}

public enum WorkState
{
    AWAY,
    WORKING
}

[System.Serializable]
public class CurriculumData
{
    public string personName;
    public string gender;
    public string age;
    public string cell;
    public string civil;
    public string vaga;
    public InformationDatabase.Vacancy vacancy;
    public string exp;
    public string salary;

    public string relationship;

    public Trait[] positiveTraits;
    public Trait[] negativeTraits;

    public bool hasFamily;
    public int sonsQtd;
    public int contributionTime;

    [Header("Appearence")]

    public float height;
    public int bodyType;
    public int hairType;
    public int headType;
    public int glassesType;
    public int noseType;
    public int mouthType;
    public int clothesType;

    public string skinHex;
    public string hairHex;
    public string clothesHex;




    [Header("Temporary Variables")]
    public bool workStateLocked;
    public WorkState workState = WorkState.WORKING;
    public int daysWorked;
    public int totalDaysWorked;

    public float temporaryAwayChance;

    public void Store(Curriculum c)
    {
        personName = c.curriculumData.personName;
        gender = c.curriculumData.gender;
        age = c.curriculumData.age;
        cell = c.curriculumData.cell;
        civil = c.curriculumData.civil;
        vaga = c.curriculumData.vaga;
        vacancy = c.curriculumData.vacancy;
        exp = c.curriculumData.exp;
        salary = c.curriculumData.salary;

        positiveTraits = c.curriculumData.positiveTraits;
        negativeTraits = c.curriculumData.negativeTraits;

        hasFamily = c.curriculumData.hasFamily;
        sonsQtd = c.curriculumData.sonsQtd;
        contributionTime = c.curriculumData.contributionTime;

        height = c.curriculumData.height;

        bodyType    = c.curriculumData.bodyType;
        hairType    = c.curriculumData.hairType;
        noseType    = c.curriculumData.noseType;
        mouthType = c.curriculumData.mouthType;
        headType = c.curriculumData.headType;
        glassesType = c.curriculumData.glassesType;
        clothesType = c.curriculumData.clothesType;

        skinHex = c.curriculumData.skinHex;
        clothesHex = c.curriculumData.clothesHex;
        hairHex = c.curriculumData.hairHex;
    }
    public void Store(PersonData data)
    {
        negativeTraits = data.c.negativeTraits;
        positiveTraits = data.c.positiveTraits;

        personName = data.c.personName;
        gender = data.c.gender;
        age = data.c.age;
        cell = data.c.cell;
        civil = data.c.civil;
        vaga = data.c.vaga;
        vacancy = data.c.vacancy;
        exp = data.c.exp;
        salary = data.c.salary;

        height = data.c.height;

        bodyType = data.c.bodyType;
        hairType = data.c.hairType;
        noseType = data.c.noseType;
        mouthType = data.c.mouthType;
        headType = data.c.headType;
        glassesType = data.c.glassesType;
        clothesType = data.c.clothesType;

        skinHex = data.c.skinHex;
        clothesHex = data.c.clothesHex;
        hairHex = data.c.hairHex;

        hasFamily = ((Random.value * 100) < 50f);
        if(hasFamily) sonsQtd = Random.Range(1,5);

    }

    public PersonData Convert()
    {
        PersonData person = new PersonData();

        return person;
    }

    public Curriculum TempCur()
    {
        Curriculum cur = new Curriculum();
        cur.curriculumData = this;
        return cur;
    }

    public Trait[] GetAllTraits()
    {
        List<Trait> traits = new List<Trait>();
        traits.AddRange(positiveTraits);
        traits.AddRange(negativeTraits);
        return traits.ToArray();
    }

    public void IncreaseDays(int days)
    {
        daysWorked += days;
        totalDaysWorked += days;
    }
    public float GetAwayChance()
    {
        float chance = 0f;
        Trait[] traits = GetAllTraits();

        for (int i = 0; i < traits.Length; i++)
        {
            chance += traits[i].awayChance;
        }
        chance += temporaryAwayChance;
        return chance;
    }

    public void AddTrait(Trait trait)
    {
        List<Trait> traits = new List<Trait>();
        traits.Add(trait);

        if (trait.type == TraitType.POSITIVE)
        {
            if(positiveTraits == null) traits.AddRange(positiveTraits);
            positiveTraits = traits.ToArray();
        }
        else
        {
            if(negativeTraits == null) traits.AddRange(negativeTraits);
            negativeTraits = traits.ToArray();
        }
    }
}
