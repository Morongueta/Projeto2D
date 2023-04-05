using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.ParticleSystem;

public class Curriculum : MonoBehaviour
{
    public string personName;
    public string gender;
    public string age;
    public string cell;
    public string civil;
    public string vaga;
    public string exp;
    public string salary;


    public Trait[] positiveTraits;
    public Trait[] negativeTraits;

    public bool hasFamily;

    public int sonsQtd;

    public int contributionTime;

    public void Set(string name, string gender, string age, string cell, string civil, string vaga, string exp, string salary)
    {
        personName = name;
        this.gender = gender;
        this.age = age;
        this.cell = cell;
        this.civil = civil;
        this.vaga = vaga;
        this.exp = exp;
        this.salary = salary;

        hasFamily = ((Random.value * 100) < 50f);
        if(hasFamily) sonsQtd = Random.Range(1,5);

        if((Random.value * 100f) < 50f)
        {
            positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, this);
            negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, this);
        }
        else
        {
            negativeTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.NEGATIVE, this);
            positiveTraits = InformationDatabase.i.GetRandomTraits(3, TraitType.POSITIVE, this);
        }
        List<Trait> traits = new List<Trait>();
        traits.AddRange(negativeTraits);
        traits.AddRange(positiveTraits);
        for (int i = 0; i < traits.Count; i++)
        {
            contributionTime += traits[i].contributionTime;
        }

        contributionTime += Random.Range(5,10);

        if(GetComponent<CurriculumUI>() != null)
        {
            GetComponent<CurriculumUI>().Set(name,gender,age,cell,civil,vaga,exp,salary);
        }
        
    }

    public void Set(CurriculumData data)
    {
        personName = data.personName;
        this.gender = data.gender;
        this.age = data.age;
        this.cell = data.cell;
        this.civil = data.civil;
        this.vaga = data.vaga;
        this.exp = data.exp;
        this.salary = data.salary;

        hasFamily = data.hasFamily;
        sonsQtd = data.sonsQtd;

        positiveTraits = data.positiveTraits;
        negativeTraits = data.negativeTraits;

        contributionTime = data.contributionTime;

        if(GetComponent<CurriculumUI>() != null)
        {
            GetComponent<CurriculumUI>().Set(name,gender,age,cell,civil,vaga,exp,salary);
        }
    }

    public CurriculumData Convert()
    {
        CurriculumData c = new CurriculumData();

        c.Store(this);

        return c;
    }

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
    public string exp;
    public string salary;

    public Trait[] positiveTraits;
    public Trait[] negativeTraits;

    public bool hasFamily;
    public int sonsQtd;
    public int contributionTime;

    public void Store(Curriculum c)
    {
        personName = c.personName;
        gender = c.gender;
        age = c.age;
        cell = c.cell;
        civil = c.civil;
        vaga = c.vaga;
        exp = c.exp;
        salary = c.salary;

        positiveTraits = c.positiveTraits;
        negativeTraits = c.negativeTraits;

        hasFamily = c.hasFamily;
        sonsQtd = c.sonsQtd;
        contributionTime = c.contributionTime;
    }

    public Trait[] GetAllTraits()
    {
        List<Trait> traits = new List<Trait>();
        traits.AddRange(positiveTraits);
        traits.AddRange(negativeTraits);
        return traits.ToArray();
    }

    public void AddTrait(Trait trait)
    {
        List<Trait> traits = new List<Trait>();
        if (trait.type == TraitType.POSITIVE)
        {
            traits.AddRange(positiveTraits);

        }
        else
        {

        }
    }
}
