using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInfo : MonoBehaviour
{
    public CurriculumData c;

    private PersonAppearance appearance;

    private void Awake()
    {
        appearance = GetComponent<PersonAppearance>();
    }

    public void SetPerson(CurriculumData c)
    {
        this.c = c;

        appearance.SetAppearance(this);
    }

    public Trait[] GetAllTraits()
    {
        List<Trait> traits = new List<Trait>();
        traits.AddRange(c.negativeTraits);
        traits.AddRange(c.positiveTraits);
        return traits.ToArray();
    }
}