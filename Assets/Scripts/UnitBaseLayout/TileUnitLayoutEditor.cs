using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileUnitLayout))]
[CanEditMultipleObjects]
public class TileUnitLayoutEditor : Editor
{
    private SerializedProperty padding;
    private SerializedProperty spacing;
    private SerializedProperty unitCount;
    private SerializedProperty startingCorner;
    private SerializedProperty startingAxis;
    private SerializedProperty unitMagnitudeReference;
    private SerializedProperty unitMagnitude;

    private TileUnitLayout m_target;

    public override void OnInspectorGUI()
    {
        OnHeaderGUI();
        serializedObject.Update();
        InitializeProperties();
        BuildEditorGui();

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnHeaderGUI()
    {
    }

    private void InitializeProperties()
    {
        m_target = target as TileUnitLayout;
        padding = serializedObject.FindProperty("padding");
        spacing = serializedObject.FindProperty("spacing");
        unitCount = serializedObject.FindProperty("unitCount");
        startingCorner = serializedObject.FindProperty("startingCorner");
        startingAxis = serializedObject.FindProperty("startingAxis");
        unitMagnitude = serializedObject.FindProperty("unitMagnitude");
        unitMagnitudeReference = serializedObject.FindProperty("unitSizeReference");
    }

    private void BuildEditorGui()
    {
        EditorGUILayout.PropertyField(padding);
        EditorGUILayout.PropertyField(spacing);
        EditorGUILayout.PropertyField(unitCount);
        EditorGUILayout.PropertyField(startingCorner);
        EditorGUILayout.PropertyField(startingAxis);

        EditorGUILayout.PropertyField(unitMagnitudeReference);
        if (unitMagnitudeReference.enumValueIndex == (int) TileUnitLayout.UnitSizeReference.ManualInput)
        {
            EditorGUILayout.PropertyField(unitMagnitude);
        }
        
        
        //Create a button for rebuilding 
        if(GUILayout.Button("Rebuild Layout"))
            m_target.RebuildLayout();
    }
}