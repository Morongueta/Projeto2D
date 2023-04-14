using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoexistenceManager))]
public class CoexistenceEditor : Editor
{
    public int personID;
    public int traitID;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CoexistenceManager co = (CoexistenceManager)target;
        
        if(GUILayout.Button("Add Person"))
        {
            co.AddPerson();
        }
        

        CurriculumData cur = co.GetPerson(personID);
        Trait trait = FindObjectOfType<InformationDatabase>().GetTrait(traitID);

        personID = EditorGUILayout.IntSlider("Person ID",personID, 0, Mathf.Max(co.GetPersons().Length - 1, 0));
        EditorGUILayout.LabelField((cur.personName != string.Empty) ? cur.personName : "Null");
        traitID = EditorGUILayout.IntSlider("Trait ID",traitID, 0, FindObjectOfType<InformationDatabase>().GetTraits().Length - 1);
        EditorGUILayout.LabelField((trait != null) ? trait.name : "Null");

        if(GUILayout.Button("Add Trait"))
        {
            if(cur.personName != string.Empty && trait != null) co.AddTrait(personID, traitID);
        }
    }
}

