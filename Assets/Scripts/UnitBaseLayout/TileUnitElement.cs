using System;
using UnityEngine;

public class TileUnitElement : MonoBehaviour
{
    [SerializeField] private int priority ;
    [Range(1,10)][SerializeField] private int tileUnits;
    private RectTransform rect;
    

    public int TileUnits
    {
        get => tileUnits;
        set => tileUnits = value;
    }
    public int Priority
    {
        get
        {
            return priority;
        }
        set => priority = value;
    }
    public RectTransform Rect
    {
        get
        {
            if(rect == null)
                rect = GetComponent<RectTransform>();
            return rect;
        }
    }
}