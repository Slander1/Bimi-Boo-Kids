using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemsCreator : MonoBehaviour
{
    public event Action<DragHandler> ItemCreated;
    public event Action<List<ItemSlot>> ItemSlotCreated;

    private ItemsRandomizer _itemOnBoardRandomizer;

    private readonly List<Item> _items = new();
    private readonly List<ItemSlot> _itemSlots = new();


    public void Init(ItemsRandomizer itemOnBoardRandomizer, Action<DragHandler> itemOnSlotPosAndNotEndGame)
    {
        _itemOnBoardRandomizer = itemOnBoardRandomizer;
        Subscribe(itemOnSlotPosAndNotEndGame);
    }


    private void Subscribe(Action<DragHandler> itemOnSlotPosAndNotEndGame)
    {
        _itemOnBoardRandomizer.RandomizeEnded += InstantiateItems;
        itemOnSlotPosAndNotEndGame += OnItemOnSlotPos;
    }

    private void OnItemOnSlotPos(DragHandler dragHandler)
    {
        _items.Remove(dragHandler.item);
        dragHandler.enabled = false;
        InstantiateMainItem();
    }

    private void InstantiateItems(List<ItemsWithSlot> itemsWithSlots)
    {
        for (int i = 0; i < itemsWithSlots.Count; i++)
        {
            var item = itemsWithSlots[i];

            _items.Add(item.itemElement);
            _itemSlots.Add(item.itemSlotElement);

            item.itemElement.groupID = i;
            item.itemSlotElement.groupID = i;
        }

        InstantiateMainItem();
        InstantiateItemsSlots();
    }

    private void InstantiateMainItem()
    {
        var index = _itemOnBoardRandomizer.ChoseRandomItem(_items.Count);

        var item = Instantiate(_items[index]);

        ItemCreated?.Invoke(item.GetComponent<DragHandler>());
    }

    private void InstantiateItemsSlots()
    {
        var instantiatesItemsSlots = new List<ItemSlot>();

        foreach (var itemSlot in _itemSlots)
        {
            var instantiateItemSlot = Instantiate(itemSlot);
            instantiatesItemsSlots.Add(instantiateItemSlot);
        }

        ItemSlotCreated?.Invoke(instantiatesItemsSlots);
    }

    public void OnDisable()
    {
        _itemOnBoardRandomizer.RandomizeEnded -= InstantiateItems;
    }
}

