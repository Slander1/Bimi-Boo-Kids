using UnityEngine;
using UnityEngine.EventSystems;
using System;


[RequireComponent(typeof(Item))]

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Item Item { get; private set; }

    private Vector3 _screenPoint;
    private Vector3 _offset;
    private Camera _camera;
    
    public event Action<DragHandler> DragEnded;
    public event Action<DragHandler, Vector3> Dragging;


    private void Awake()
    {
         Item = GetComponent<Item>();
        _camera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var position = transform.position;
        _screenPoint = _camera.WorldToScreenPoint(position);
        _offset = position - _camera.ScreenToWorldPoint(new Vector3(eventData.position.x,
            eventData.position.y, _screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
        var cursorPoint = new Vector3(eventData.position.x, eventData.position.y, _screenPoint.z);
        var cursorPosition = _camera.ScreenToWorldPoint(cursorPoint) + _offset;
        var deltaPosition = cursorPosition - transform.position; ;
        Dragging?.Invoke(this, deltaPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragEnded?.Invoke(this);
    }
}

