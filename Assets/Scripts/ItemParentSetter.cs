using UnityEngine;
using System.Collections.Generic;

public class ItemParentSetter : MonoBehaviour
{
	[SerializeField] private RectTransform itemContainer;
    [SerializeField] private RectTransform[] itemsSlotsContainer;

    private ItemsPositioner _itemsPositioner;
    private ItemsCreator _itemsCreator;

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
        _itemsPositioner.ItemOnSlotPos += OnItemOnSlotPos;
    }

    private void OnItemOnSlotPos(DragHandler dragHandler, ItemSlot itemSlot)
    {
        var itemTransform = dragHandler.transform;
        itemTransform.SetParent(itemSlot.transform, true);
        SetTransform(itemTransform);
    }

    private void OnItemCreated(DragHandler dragHandler)
    {
        var itemTransform = dragHandler.transform;
        itemTransform.SetParent(itemContainer, true);
        SetTransform(itemTransform);
    }

    private void OnItemSlotCreated(List<ItemSlot> itemsSlots)
    {
        for (int i = 0; i < itemsSlots.Count; i++)
        {
            var itemSlotTransform = itemsSlots[i].transform;
            itemSlotTransform.SetParent(itemsSlotsContainer[i], true);
            SetTransform(itemSlotTransform);
        }
    }

    private static void SetTransform(Transform pos)
    {
        pos.localPosition = new Vector3(0, 0, -1);
    }

}

