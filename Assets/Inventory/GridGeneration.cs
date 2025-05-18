using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGeneration : MonoBehaviour
{
    [SerializeField, Foldout("Grid Size")] private int width, height;
    [Space(5)]
    [SerializeField] private GridTile tilePrefab;
    
    private int tileSize = 64;

    private void Awake()
    {
        HandleResize();
        
        var gridGroup = this.GetComponent<GridLayoutGroup>();
        gridGroup.cellSize = Vector2.one * tileSize;
        
        var parentRoot = this.transform;

        for (var i = 0; i < (width * height); i++)
        {
            var tileInstance = Instantiate(tilePrefab, parentRoot);
            tileInstance.InitializeTile(i / width, (i % width));
        }
    }

    private void HandleResize()
    {
        if (!TryGetComponent<RectTransform>(out var frameRef)) return;
        var frameRect = frameRef.rect;
            
        var cellWidth = (int) frameRect.width / width;
        var cellHeight = (int) frameRect.height / height;

        var resizeTarget = transform.parent.GetComponent<RectTransform>();
        if (cellHeight > cellWidth)
        {
            resizeTarget.sizeDelta = new Vector2(cellHeight * width, frameRect.height);
            tileSize = cellHeight;
        }
        else if (cellHeight < cellWidth)
        {
            resizeTarget.sizeDelta = new Vector2(frameRect.width, cellWidth * height);
            tileSize = cellWidth;
        }
        else
        {
            tileSize = cellWidth;
        }
    }
}
