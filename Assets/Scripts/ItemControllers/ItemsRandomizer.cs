using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ItemControllers
{
    public class ItemsRandomizer
    {
        public event Action<List<ItemsWithSlot>> RandomizeEnded;

        public ItemsRandomizer(int seed)
        {
            Random.InitState(seed);
        }

        public void RandomizeItem(IEnumerable<ItemsWithSlot> itemWithSlots, int countSlotsOnBoard)
        {
            var itemsWithSlotCopy = new List<ItemsWithSlot>(itemWithSlots.ToList());
        
            var currentGameItemsWithSlots = new List<ItemsWithSlot>();

            while (currentGameItemsWithSlots.Count < countSlotsOnBoard)
            {
                var index = Random.Range(0, itemsWithSlotCopy.Count);
                var itemWithSlot = itemsWithSlotCopy[index];
                itemsWithSlotCopy.Remove(itemWithSlot);
                currentGameItemsWithSlots.Add(itemWithSlot);
            }

            RandomizeEnded?.Invoke(currentGameItemsWithSlots);
        }

        public int ChoseRandomItem(int count)
        {
            return Random.Range(0, count);
        }

    }
}
