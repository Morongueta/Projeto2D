using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InformationDatabase))]
public class InformationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        

        InformationDatabase info = (InformationDatabase)target;

        for (int i = 0; i < info.traits.Length; i++)
        {
            info.traits[i].ID = i;
        }
    }
}