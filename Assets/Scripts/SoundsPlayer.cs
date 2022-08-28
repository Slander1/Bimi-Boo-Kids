using Cysharp.Threading.Tasks;
using ItemControllers;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource itemAppear;
    [SerializeField] private AudioSource itemSuccess;

    public void Init(ItemAnimatedMover itemAnimatedMover, ItemsPositioner itemsPositioner)
    {
        Subscribe(itemAnimatedMover, itemsPositioner);
    }

    private void Subscribe(ItemAnimatedMover itemAnimatedMover, ItemsPositioner itemsPositioner)
    {
        itemAnimatedMover.ItemAppeared += OnItemAppeared;
        itemsPositioner.ItemSuccessed += OnItemSuccessed;
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

 
}