using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool _isDragging = false;
    private bool _isRotating = false;

    private Image targetVisual;
    
    private void Awake()
    {
        targetVisual = GetComponentInChildren<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!_isDragging) _isDragging = true;
            
            this.transform.DOMove(Input.mousePosition, .05f)
                .SetEase(Ease.OutBounce);

            targetVisual.raycastTarget = false;
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_isDragging) _isDragging = false;

            targetVisual.raycastTarget = true;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        
        this.transform.position = Input.mousePosition;
    }
    
    private void Update()
    {
        if (!_isDragging)
            return;
        if (!Input.GetMouseButtonUp(1))
            return;
        if (_isRotating)
            return;
        
        RotateOnce();
    }
    
    private void RotateOnce()
    {
        _isRotating = true;
        this.transform.DORotate(Vector3.forward * (-90), .1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => _isRotating = false);
    }
}
