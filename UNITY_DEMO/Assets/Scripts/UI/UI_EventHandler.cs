using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnPointerClickHandler = null;
    public Action<PointerEventData> OnDragHanler = null; 

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHanler != null)
            OnDragHanler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerClickHandler != null)
            OnPointerClickHandler.Invoke(eventData);
    }
}
