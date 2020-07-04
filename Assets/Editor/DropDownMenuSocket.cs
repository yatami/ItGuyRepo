using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SocketFunction))]
public class DropDownMenuSocket : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SocketFunction script = (SocketFunction)target;

        GUIContent array = new GUIContent("Color Choice");
        script.colorArrIndex = EditorGUILayout.Popup(array, script.colorArrIndex, script.colorArr);
        EditorUtility.SetDirty(target);
    }
}
