using System;
using Cysharp.Threading.Tasks;
using ItemControllers;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource itemAppear;
    [SerializeField] private AudioSource itemSuccess;

    private ItemAnimatedMover _itemAnimatedMover;
    private ItemsPositioner _itemsPositioner;

    public void Init(ItemAnimatedMover itemAnimatedMover, ItemsPositioner itemsPositioner)
    {
        _itemAnimatedMover = itemAnimatedMover;
        _itemsPositioner = itemsPositioner;
        Subscribe();
    }

    private void Subscribe()
    {
        _itemAnimatedMover.ItemAppeared += OnItemAppeared;
        _itemsPositioner.ItemSuccessed += OnItemSuccessed;
    }

    private  async void OnItemSuccessed(DragHandler dragHandler, ItemSlot itemSlot)
    {
        await UniTask.WaitUntil(() => (!itemAppear.isPlaying) && (!itemSuccess.isPlaying));
        itemSuccess.Play();
    }

    private async void OnItemAppeared()
    {
        await UniTask.WaitUntil(() => (!itemAppear.isPlaying) && (!itemSuccess.isPlaying));
        itemAppear.Play();
    }

    private void OnDisable()
    {
        _itemAnimatedMover.ItemAppeared -= OnItemAppeared;
        _itemsPositioner.ItemSuccessed -= OnItemSuccessed;
    }
}