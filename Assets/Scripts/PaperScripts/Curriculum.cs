using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Curriculum : MonoBehaviour
{
    public TextMeshPro nameText;
    public TextMeshPro genderText;
    public TextMeshPro ageText;
    public TextMeshPro cellText;
    public TextMeshPro civilText;
    public TextMeshPro vagasText;
    public TextMeshPro expText;
    public TextMeshPro salaryText;

    public Trait[] positiveTraits;
    public Trait[] negativeTraits;

    public bool hasFamily;

    public void Set(string name, string gender, string age, string cell, string civil, string vaga, string exp, string salary)
    {
        nameText.text = name;
        genderText.text = gender;
        ageText.text = age;
        cellText.text = cell;
        civilText.text = civil;
        vagasText.text = vaga;
        expText.text = exp;
        salaryText.text = salary;

        hasFamily = ((Random.value * 100) < 50f);

        positiveTraits = InformationDatabase.i.GetRandomTraits(3,TraitType.POSITIVE);
        negativeTraits = InformationDatabase.i.GetRandomTraits(3,TraitType.NEGATIVE);
    }

}
