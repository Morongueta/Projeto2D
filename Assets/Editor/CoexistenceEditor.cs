using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoexistenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CoexistenceManager co = (CoexistenceManager)target;


    }
}

