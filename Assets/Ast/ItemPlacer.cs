using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private Vector2Int itemSize;

    [SerializeField] private RectTransform[] objTiles;
    [SerializeField] private int tileSize;
    private bool[,] shapeMapper;

    private void Awake()
    {
        shapeMapper = new bool[itemSize.x, itemSize.y];

        for (var i = 0; i < objTiles.Length; i++)
        {
            var coordX = (int) objTiles[i].position.x;
            var coordY = (int) objTiles[i].position.y;
            Debug.Log($"{objTiles[i].name} : {coordX / tileSize}, {coordY / tileSize}");
            // shapeMapper[coordX / tileSize, coordY / tileSize] = true;

            
        }
    }

    private List<Transform> GetShape()
    {
        return null;
    }
}
