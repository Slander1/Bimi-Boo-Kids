using Cysharp.Threading.Tasks;
using ItemControllers;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource itemAppear;
    [SerializeField] private AudioSource itemSuccess;

    public void Init(ItemsCreator itemsCreator, ItemsPositioner itemsPositioner)
    {
        Subscribe(itemsCreator, itemsPositioner);
    }

    private void Subscribe(ItemsCreator itemsCreator, ItemsPositioner itemsPositioner)
    {
        itemsCreator.ItemCreated += OnItemCreated;
        itemsPositioner.ItemOnSlotPos += OnItemSuccess;
    }

    private  async void OnItemSuccess(DragHandler dragHandler, ItemSlot itemSlot)
    {
        await UniTask.WaitUntil(() => (!itemAppear.isPlaying) && (!itemSuccess.isPlaying));
        itemSuccess.Play();
    }

    private async void OnItemCreated(DragHandler dragHandler)
    {
        await UniTask.WaitUntil(() => (!itemAppear.isPlaying) && (!itemSuccess.isPlaying));
        itemAppear.Play();
    }

 
}