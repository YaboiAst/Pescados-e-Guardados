using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector2Int _coord;

    private Image tileColor;
    
    public void InitializeTile(int xIndex, int yIndex)
    {
        _coord = new Vector2Int(xIndex, yIndex);
        
        
        tileColor = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            var item = eventData.pointerDrag.GetComponent<ItemPlacer>();
            
        }
        return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            tileColor.color = Color.white;
        }
        return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_coord);
    }
}
