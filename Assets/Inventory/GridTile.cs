using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector2Int _coord;
    
    public void InitializeTile(int xIndex, int yIndex)
    {
        _coord = new Vector2Int(xIndex, yIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_coord);
    }
}
