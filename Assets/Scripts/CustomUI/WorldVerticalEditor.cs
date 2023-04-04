using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WorldVerticalGroup))]
public class WorldVerticalEditor : Editor {
    public override void OnInspectorGUI() {
        
        base.OnInspectorGUI();
        if(GUILayout.Button("Draw"))
        {
            ((WorldVerticalGroup)target).Vertical();
        }
        ((WorldVerticalGroup)target).Vertical();
    }
}
