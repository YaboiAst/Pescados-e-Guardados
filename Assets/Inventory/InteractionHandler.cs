using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false; 
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging) isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging) isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        transform.position = Input.mousePosition + (Vector3.forward * 9);
    }
}
