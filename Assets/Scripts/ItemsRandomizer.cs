using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class ItemsRandomizer
{
    public event Action<List<ItemsWithSlot>> RandomizeEnded;

    public ItemsRandomizer(int seed)
    {
        Random.InitState(seed);
    }

    public void RandomizeItem(List<ItemsWithSlot> itemWithSlots, int countSlotsOnBoard)
    {
        
        var currentGameItemsWithSlots = new List<ItemsWithSlot>();

        while (currentGameItemsWithSlots.Count < countSlotsOnBoard)
        {
            var index = Random.Range(0, itemWithSlots.Count);
            var itemWithSlot = itemWithSlots[index];
            itemWithSlots.Remove(itemWithSlot);
            currentGameItemsWithSlots.Add(itemWithSlot);
        }

        RandomizeEnded?.Invoke(currentGameItemsWithSlots);
    }

    public int ChoseRandomItem(int count)
    {
        return Random.Range(0, count);
    }

}
