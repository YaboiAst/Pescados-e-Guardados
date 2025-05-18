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
    
    private Vector2Int _tileSize = Vector2Int.one * 64;

    private void Awake()
    {
        HandleResize();
        
        var gridGroup = this.GetComponent<GridLayoutGroup>();
        gridGroup.cellSize = Vector2.one * _tileSize;
        
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
        if (!TryGetComponent<GridLayoutGroup>(out var gridGroup)) return;
            
        var cellWidth = (int) frameRect.width / width;
        var cellHeight = (int) frameRect.height / height;

        _tileSize = new Vector2Int(cellWidth, cellHeight);
    }
}
