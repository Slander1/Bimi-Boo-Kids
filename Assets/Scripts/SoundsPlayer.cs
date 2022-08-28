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

    private void OnItemSuccess(DragHandler dragHandler, ItemSlot itemSlot)
    {
        itemSuccess.Play();
    }

    private void OnItemCreated(DragHandler dragHandler)
    {
        itemAppear.Play();
    }
}