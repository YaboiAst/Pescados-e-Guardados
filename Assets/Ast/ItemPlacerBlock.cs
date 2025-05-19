using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacerBlock : MonoBehaviour
{
    private RectTransform rect;
    public Rect GetRect()
    {
        var w = rect.rect.width;
        var h = rect.rect.height;
        var r = new Rect(rect.position.x + (w/2), rect.position.y - (h/2), w/2.2f, h/2.2f);
        return r;
    }
    
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; 
        
        var r = GetRect();
        Gizmos.DrawWireCube(r.position, new Vector3(r.width, r.height, 1));
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
}
