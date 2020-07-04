using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlugControl))]
public class DropDownMenuPlug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlugControl script = (PlugControl)target;

        GUIContent array = new GUIContent("Color Choice");
        script.colorArrIndex = EditorGUILayout.Popup(array, script.colorArrIndex, script.colorArr);
        EditorUtility.SetDirty(target);
    }
}
