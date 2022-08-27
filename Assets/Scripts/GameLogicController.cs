using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class GameLogicController : MonoBehaviour
{
    [SerializeField] private ItemsWithSlot[] itemsWithSlots;

    [SerializeField] private int seed = 1;

    [SerializeField] private int countSlotsOnBoard = 3;

    [SerializeField] private ItemsCreator _itemCreator;

    private ItemsRandomizer _itemOnBoardRandomizer;


    public void Awake()
    {
        _itemOnBoardRandomizer = new ItemsRandomizer(seed);

        _itemCreator.Init(_itemOnBoardRandomizer);

        _itemOnBoardRandomizer.RandomizeItem(itemsWithSlots.ToList(), countSlotsOnBoard);
    }

}

