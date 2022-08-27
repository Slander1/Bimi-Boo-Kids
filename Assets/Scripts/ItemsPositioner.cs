using UnityEngine;
using System.Collections;

public class ItemsPositioner
{
    private ItemsCreator _itemsCreator;


    private void Init(ItemsCreator itemsCreator)
    {
        _itemsCreator = itemsCreator;
        Subscribe();
    }


    private void Subscribe()
    {
        _itemsCreator.ItemCreated += InstantiateItems;
        _itemsCreator.ItemSlotCreated += InstantiateItems;
    }
}

