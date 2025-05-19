using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private List<ItemPlacerBlock> blocksRects;
    public List<Rect> GetRects()
    {
        var rectList = new List<Rect>();
        foreach (var block in blocksRects)
        {
            rectList.Add(block.GetRect());
        }
        return rectList;
    }
}
