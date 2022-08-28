using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemControllers
{
    public class ItemParentSetter : MonoBehaviour
    {
        [SerializeField] private RectTransform itemContainer;
        [SerializeField] private RectTransform[] itemsSlotsContainer;
        
        [SerializeField] private RectTransform slotCreatePoint;
        [SerializeField] private RectTransform itemCreatePoint;
        
        private ItemsPositioner _itemsPositioner;
        private ItemsCreator _itemsCreator;

        public event Action<Transform> ParentSet; 
        public event Action<DragHandler, bool> ItemParentSet; 

        public void Init(ItemsPositioner itemsPositioner, ItemsCreator itemsCreator)
        {
            _itemsPositioner = itemsPositioner;
            _itemsCreator = itemsCreator;
            Subscribe();
        }

        private void Subscribe()
        {
            _itemsCreator.ItemCreated += OnItemCreated;
            _itemsCreator.ItemSlotCreated += OnItemSlotCreated;
            _itemsPositioner.ItemSuccessed += OnItemSuccessed;
        }
        
        private void OnDisable()
        {
            _itemsCreator.ItemCreated -= OnItemCreated;
            _itemsCreator.ItemSlotCreated -= OnItemSlotCreated;
            _itemsPositioner.ItemSuccessed -= OnItemSuccessed;
        }

        private void OnItemSuccessed(DragHandler dragHandler, ItemSlot itemSlot)
        {
            var itemTransform = dragHandler.transform;
            itemTransform.SetParent(itemSlot.transform, true);
        }

        private void OnItemCreated(DragHandler dragHandler)
        {
            var itemTransform = dragHandler.transform;
            itemTransform.SetParent(itemContainer, true);
            dragHandler.transform.position = itemCreatePoint.transform.position;
            ItemParentSet?.Invoke(dragHandler, true);
        }

        private void OnItemSlotCreated(List<ItemSlot> itemsSlots)
        {
            for (var i = 0; i < itemsSlots.Count; i++)
            {
                var itemSlotTransform = itemsSlots[i].transform;
                itemSlotTransform.SetParent(itemsSlotsContainer[i], true);
                itemSlotTransform.position = slotCreatePoint.transform.position;
                ParentSet?.Invoke(itemSlotTransform);
            }
        }
    }
}

