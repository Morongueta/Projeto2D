using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInfo : MonoBehaviour
{
    public string personName;
    public Trait[] positiveTraits;
    public Trait[] negativeTraits;
    public bool hasFamily;
    #region Family variables
    public int sonsQtd;
    public int dogsQtd;
    public int catsQtd; //So pa zua
    #endregion

    public void SetPerson(Curriculum c)
    {
        personName = c.nameText.text;
        positiveTraits = c.positiveTraits;
        negativeTraits = c.negativeTraits;
        hasFamily = c.hasFamily;

        if(hasFamily)
        {
            sonsQtd = Random.Range(1,5);
            dogsQtd = Random.Range(0,3);
            catsQtd = Random.Range(0,4);
        }
    }
}