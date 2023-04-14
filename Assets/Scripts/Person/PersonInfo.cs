using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInfo : MonoBehaviour
{
    public string personName;
    public Trait[] positiveTraits;
    public Trait[] negativeTraits;
    public bool hasFamily;
    public int sonsQtd;
    public int contributionTime;

    public Curriculum c;

    public void SetPerson(Curriculum c)
    {
        personName = c.personName;
        positiveTraits = c.positiveTraits;
        negativeTraits = c.negativeTraits;
        hasFamily = c.hasFamily;
        contributionTime = c.contributionTime;

        sonsQtd = c.sonsQtd;

        

        this.c = c;
    }

    public Trait[] GetAllTraits()
    {
        List<Trait> traits = new List<Trait>();
        traits.AddRange(negativeTraits);
        traits.AddRange(positiveTraits);
        return traits.ToArray();
    }
}