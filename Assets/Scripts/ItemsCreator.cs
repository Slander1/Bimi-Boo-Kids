using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemsCreator : MonoBehaviour
{
    public event Action<Item> ItemCreated;
    public event Action<List<ItemSlot>> ItemSlotCreated;

    private ItemsRandomizer _itemOnBoardRandomizer;

    private List<Item> _items = new List<Item>();
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();


    public void Init(ItemsRandomizer itemOnBoardRandomizer)
    {
        _itemOnBoardRandomizer = itemOnBoardRandomizer;
        Subscribe();
    }


    private void Subscribe()
    {
        _itemOnBoardRandomizer.RandomizeEnded += InstantiateItems;
    }

    private void InstantiateItems(List<ItemsWithSlot> itemsWithSlots)
    {
        foreach (var itemWithSlot in itemsWithSlots)
        {
            _items.Add(itemWithSlot.ItemElement);
            _itemSlots.Add(itemWithSlot.ItemSlotElement);
        }

        InstantiateMainItem();
        InstantiateItemsSlots();
    }

    private void InstantiateMainItem()
    {
        var index = _itemOnBoardRandomizer.ChoseRandomItem(_items.Count);

        var item = Instantiate(_items[index]);

        ItemCreated?.Invoke(item);
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

