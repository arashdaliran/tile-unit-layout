using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TileUnitLayout : MonoBehaviour
{
    [SerializeField] private RectOffset padding = new RectOffset();
    [SerializeField] private float spacing;
    [SerializeField] private int unitCount;
    [SerializeField] private Corner startingCorner;
    [SerializeField] private Axis startingAxis;

    [SerializeField] private bool forceUpdate;
    [SerializeField] private UnitSizeReference unitSizeReference;
    [SerializeField] private float unitMagnitude;
    
    private int lastStateChildCount;
    //private List<int> rowsCapacity = new List<int>();
    private int rowCapacity;
    private List<TileUnitElement> elements = new List<TileUnitElement>();
    private Vector2Int signMultiplier;
    private Vector2Int startPadding;
    private Vector2 currentSlotPosition;
    private Vector2 pivotAdditive;
    private Vector2 anchorPosition;
    
    private RectTransform Rect  => GetComponent<RectTransform>();
    private void Update()
    {
        if (!transform.childCount.Equals(lastStateChildCount))
        {
            ChildCountChanged();
            lastStateChildCount = transform.childCount;
        }

        //print($"is calculating the mag : {unitSizeReference}");
        if (forceUpdate)
        {
        }
    }
    
    private void ChildCountChanged()
    {
        print("Children Count Changed");
        if(unitSizeReference == UnitSizeReference.FitToParentDimension)
            ReImportElements();
        SortElements();
        ReArrangeChildren();
    }

    private void SortElements()
    {
        var sorted = elements.OrderBy(element => element.Priority);
        elements = sorted.ToList();
        FixChildSiblingIndex();
    }

    private void FixChildSiblingIndex()
    {
        foreach (var tileUnitElement in elements)
        {
            tileUnitElement.transform.SetAsLastSibling();
        }
    }

    private void ReImportElements()
    {
        elements.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            var element = t.GetComponent<TileUnitElement>();
            if (element == null)
            {
                element = t.gameObject.AddComponent<TileUnitElement>();
                element.Priority = i == 0 ? 0 : elements[elements.Count - 1].Priority;
                element.TileUnits = 1;
            }
            elements.Add(element);
        }
    }

    private void ReArrangeChildren()
    {
        ResetSignAndStartPadding();
        CalculateUnitMagnitude();
        foreach (var element in elements)
        {  
            var rect = element.Rect;
            rect.anchorMax = anchorPosition;
            rect.anchorMin = anchorPosition;
            SetElementSize(element);
            SetNextSlotPosition(element);
            SetElementPosition(rect);
        }
    }


    private void SetElementSize(TileUnitElement element)
    {
        var extendedSize = element.TileUnits * unitMagnitude + (element.TileUnits - 1) * spacing; var elementRect = element.Rect.rect;
        switch (startingAxis)
        {
            case Axis.Horizontal:
                element.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,unitMagnitude);
                element.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,extendedSize);
                break;
            case Axis.Vertical:
                element.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,extendedSize);
                element.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,unitMagnitude);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CalculateUnitMagnitude()
    {
        float directionParentMagnitude;
        int pads;
        switch (startingAxis)
        {
            case Axis.Horizontal:
                directionParentMagnitude = Rect.rect.width;
                pads = padding.horizontal;
                break;
            case Axis.Vertical:
                directionParentMagnitude = Rect.rect.height;
                pads = padding.vertical;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        unitMagnitude = (directionParentMagnitude - pads - spacing * (unitCount - 1)) / unitCount;
    }
    private void SetElementPosition(RectTransform elementRect)
    {
        var pos =currentSlotPosition + (pivotAdditive + elementRect.pivot) * elementRect.sizeDelta;
        elementRect.anchoredPosition = pos;
    }

    private void SetNextSlotPosition(TileUnitElement element)
    {
        float x;
        float y;
        if (element.TileUnits > rowCapacity)
        {
            //PointToNextRow();
            switch (startingAxis)
            {
                case Axis.Horizontal:
                    currentSlotPosition = new Vector2(signMultiplier.x * startPadding.x, 
                        currentSlotPosition.y + signMultiplier.y * (unitMagnitude + spacing));
                    break;
                case Axis.Vertical:
                    currentSlotPosition = 
                        new Vector2(currentSlotPosition.x +signMultiplier.x * (unitMagnitude + spacing), 
                        signMultiplier.y * startPadding.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            rowCapacity = unitCount;
        }
        var unitEquipped =unitCount - rowCapacity;
        var offset = unitEquipped * (unitMagnitude + spacing);
        switch (startingAxis)
        {
            case Axis.Horizontal:
                 x =startPadding.x + signMultiplier.x * offset;
                 y = currentSlotPosition.y;
                 break;
            case Axis.Vertical:
                x =currentSlotPosition.x ;
                y = startPadding.y + signMultiplier.y * offset;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        currentSlotPosition = new Vector2(x,y);
        rowCapacity -= element.TileUnits;
    }
    private void ResetSignAndStartPadding()
    {
        switch (startingCorner)
        {
            case Corner.UpperLeft:
                signMultiplier = new Vector2Int(1, -1);
                pivotAdditive = new Vector2(0,-1);
                startPadding = new Vector2Int(padding.left, padding.top);
                anchorPosition = new Vector2(0,1);
                break;
            case Corner.UpperRight:
                signMultiplier = new Vector2Int(-1, -1);
                pivotAdditive = new Vector2(-1,-1);
                startPadding = new Vector2Int(padding.right, padding.top);
                anchorPosition = new Vector2(1,1);
                break;
            case Corner.LowerLeft:
                signMultiplier = new Vector2Int(1, 1);
                pivotAdditive = new Vector2(0,0);
                startPadding = new Vector2Int(padding.left, padding.bottom);
                anchorPosition = new Vector2(0,0);
                break;
            case Corner.LowerRight:
                signMultiplier = new Vector2Int(-1, 1);
                pivotAdditive = new Vector2(-1,0);
                startPadding = new Vector2Int(padding.right, padding.bottom);
                anchorPosition = new Vector2(1,0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        currentSlotPosition = new Vector2(signMultiplier.x * startPadding.x, 
            signMultiplier.y * startPadding.y);
        rowCapacity = unitCount;
    }
    
    private void OnValidate()
    {
        ReArrangeChildren();
    }

    private enum Axis
    {
        Horizontal,
        Vertical
    }

    private enum Corner
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight
    }

    public enum UnitSizeReference
    {
        FitToParentDimension,
        ManualInput
    }
}