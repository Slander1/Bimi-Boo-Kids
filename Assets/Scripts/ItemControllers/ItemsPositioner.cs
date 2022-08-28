using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemControllers
{
    public class ItemsPositioner 
    {
        private readonly ItemsCreator _itemsCreator;
        private List<ItemSlot> _itemsSlots;

        public event Action<DragHandler, ItemSlot> ItemSuccessed;
        
        public event Action<DragHandler> ItemNotSuccessed;
        

        public ItemsPositioner(ItemsCreator itemsCreator)
        {
            _itemsCreator = itemsCreator;
            Subscribe();
        }


        private void Subscribe()
        {
            _itemsCreator.ItemCreated += OnItemCreated ;
            _itemsCreator.ItemSlotCreated += OnItemSlotCreated;
        }

        private void OnItemCreated(DragHandler dragHandler)
        {
            SubscribeOnDrag(dragHandler);
        }

        private void SubscribeOnDrag(DragHandler dragHandler)
        {
            dragHandler.Dragging += OnDragging;
            dragHandler.DragEnded += OnDragEnded;
        }

        private void UnSubscribeOnDrag(DragHandler dragHandler)
        {
            dragHandler.Dragging -= OnDragging;
            dragHandler.DragEnded -= OnDragEnded;
        }


        private void OnDragEnded(DragHandler dragHandler)
        {
            var requiredSlot = _itemsSlots.Find(itemSlot => itemSlot.groupID == dragHandler.Item.groupID);
            if (IsNear(requiredSlot.transform, dragHandler.Item.transform))
            { 
                ItemSuccessed?.Invoke(dragHandler, requiredSlot);
                dragHandler.transform.localPosition = new Vector3(0, 0, -4);
                UnSubscribeOnDrag(dragHandler);
            }
            else
                ItemNotSuccessed?.Invoke(dragHandler);
        }

        private void OnDragging(DragHandler dragHandler, Vector3 dragPos)
        {
            dragHandler.transform.position += dragPos;
        }

        private bool IsNear(Transform slot, Transform item)
        {
            return (Math.Abs(slot.position.x - item.position.x) < 0.5f) &&
                   (Math.Abs(slot.position.y - item.position.y) < 0.5f);  
        }

        private void OnItemSlotCreated(List<ItemSlot> itemsSlots)
        {
            _itemsSlots = itemsSlots;
        }
        public void Unsubscribe()
        {
            _itemsCreator.ItemCreated -= OnItemCreated ;
            _itemsCreator.ItemSlotCreated -= OnItemSlotCreated;
        }
    }
}

