using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurriculumUI : MonoBehaviour
{
    public TextMeshPro nameText;
    public TextMeshPro genderText;
    public TextMeshPro ageText;
    public TextMeshPro cellText;
    public TextMeshPro civilText;
    public TextMeshPro vagasText;
    public TextMeshPro expText;
    public TextMeshPro salaryText;


    public void Set(CurriculumData data)
    {
        nameText.text = data.personName;
        genderText.text = data.gender;
        ageText.text = data.age;
        cellText.text = data.cell;
        civilText.text = data.civil;
        vagasText.text = data.vaga;
        expText.text = data.exp;
        salaryText.text = "R$"+ data.salary;
    }
}
