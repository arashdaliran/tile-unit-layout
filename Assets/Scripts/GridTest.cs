using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GridTest : LayoutGroup
{
    public override void CalculateLayoutInputVertical()
    {
        
        throw new System.NotImplementedException();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
    }

    public override void SetLayoutHorizontal()
    {
        throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        throw new System.NotImplementedException();
    }
}
[CustomEditor(typeof(GridTest))]
[CanEditMultipleObjects]
public class GridTestEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnValidate()
    {
        throw new NotImplementedException();
    }
}
