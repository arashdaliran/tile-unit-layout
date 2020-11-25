using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileUnitElement))]
public class TileUnitElementEditor : Editor
{
    private SerializedProperty priority; 
    private SerializedProperty tileUnits; 
    public override void OnInspectorGUI()
    {
        priority = serializedObject.FindProperty("priority");
        tileUnits = serializedObject.FindProperty("tileUnits");
        EditorGUILayout.PropertyField(priority);
        EditorGUILayout.PropertyField(tileUnits);
        serializedObject.ApplyModifiedProperties();
    }
}