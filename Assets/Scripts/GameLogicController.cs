﻿using UnityEngine;
using System;
using ItemControllers;

public class GameLogicController : MonoBehaviour
{
    [SerializeField] private ItemsWithSlot[] itemsWithSlots;

    [SerializeField] private int seed = 1;

    [SerializeField] private int countSlotsOnBoard = 3;

    [SerializeField] private ItemsCreator itemCreator;
    [SerializeField] private ItemParentSetter itemParentSetter;
    [SerializeField] private SoundsPlayer soundsPlayer;
    [SerializeField] private ItemAnimatedMover itemAnimatedMover;
    [SerializeField] private RectTransform victorySqreen;

    private ItemsRandomizer _itemOnBoardRandomizer;
    private ItemsPositioner _itemsPositioner;

    private int _count = 0;

    public event Action<DragHandler> ItemOnSlotPosAndNotEndGame;

    public void Awake()
    {
        _itemOnBoardRandomizer = new ItemsRandomizer(seed);
        _itemsPositioner = new ItemsPositioner(itemCreator);
        
        itemParentSetter.Init(_itemsPositioner, itemCreator);
        itemCreator.Init(_itemOnBoardRandomizer, this);
        soundsPlayer.Init(itemCreator, _itemsPositioner);
        itemAnimatedMover.Init(itemParentSetter);
        
        _itemOnBoardRandomizer.RandomizeItem(itemsWithSlots, countSlotsOnBoard);
    }

    private void OnEnable()
    {
        _itemsPositioner.ItemOnSlotPos += OnItemOnSlotPos;
    }

    private void OnDisable()
    {
        _itemsPositioner.ItemOnSlotPos -= OnItemOnSlotPos;
    }
    private void OnItemOnSlotPos(DragHandler dragHandler, ItemSlot itemSlot)
    {
        _count++;
        if (_count == countSlotsOnBoard)
            victorySqreen.gameObject.SetActive(true);
        else
            ItemOnSlotPosAndNotEndGame?.Invoke(dragHandler);
    }

    
}

