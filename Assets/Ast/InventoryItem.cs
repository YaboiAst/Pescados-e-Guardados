using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler
{
    public DraggableItem occupied;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entrou");
    }
    
}
