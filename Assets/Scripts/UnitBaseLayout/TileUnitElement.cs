using System;
using UnityEngine;

public class TileUnitElement : MonoBehaviour
{
    [SerializeField] private int priority ;
    [SerializeField] private int tileUnits;
    private RectTransform rect;
    private TileUnitLayout parentLayout;

    private int check_priority;
    private int check_tileUnits;
    private TileUnitLayout ParentLayout
    {
        get
        {
            if (parentLayout == null)
                parentLayout = GetComponentInParent<TileUnitLayout>() ??
                               throw new MissingComponentException("There is no TileUnitLayout component at parent.");
            
            return parentLayout;
        }
    } 
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
    
    public void OnValidate()
    {
        if (tileUnits > ParentLayout.UnitCount)
            tileUnits = ParentLayout.UnitCount;
        if (tileUnits < 1)
            tileUnits = 1;
        if (tileUnits != check_tileUnits)
        {
            ParentLayout.ReArrangeChildren();
            check_tileUnits = tileUnits;
        }

        if (priority != check_priority)
        {
            ParentLayout.SortAndReArrange();
            check_priority = priority;
        }
    }
}