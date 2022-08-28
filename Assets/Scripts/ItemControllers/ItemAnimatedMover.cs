using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ItemControllers
{
    public class ItemAnimatedMover : MonoBehaviour
    {
        [SerializeField] private AnimationCurve moveCurve;
                
        private ItemParentSetter _itemParentSetter;
        private ItemsPositioner _itemsPositioner;
        private readonly Queue<Transform> _dragHandlers = new();

        public event Action ItemAppeared; 

        public void Init(ItemParentSetter itemParentSetter, ItemsPositioner itemsPositioner)
        {
            _itemParentSetter = itemParentSetter;
            _itemsPositioner = itemsPositioner;
            Subscribe();
        }
        
        private void Subscribe()
        {
            _itemParentSetter.ParentSet += OnSlotParentSet;
            _itemParentSetter.ItemParentSet += OnItemParentSet;
            _itemsPositioner.ItemNotSuccessed += OnItemNotSuccessed;
        }

        private async void OnItemNotSuccessed(DragHandler dragHandler)
        {
            dragHandler.enabled = false;
            await MoveToPosition(dragHandler.transform, true);
            dragHandler.enabled = true;
        }

        private async void OnSlotParentSet(Transform slotTransform)
        {
            await MoveToPosition(slotTransform);
        }

        private async void OnItemParentSet(DragHandler dragHandler)
        {
            dragHandler.enabled = false;
            await MoveToPosition(dragHandler.transform, true, true);
            dragHandler.enabled = true;
        }

        private async UniTask MoveToPosition(Transform transformItem, bool isItem = false, bool isAppear = false)
        {
            
            _dragHandlers.Enqueue(transformItem);
            
            await UniTask.WaitWhile(() => _dragHandlers.Peek() != transformItem);
            
            if (isItem && isAppear)
                ItemAppeared?.Invoke();
            
            var z = (isItem) ? -4 : -2;
            
            while (( transformItem.localPosition.x > 0.2f) &&
                   ( transformItem.localPosition.y > 0.2f))
            {
                var localPosition = transformItem.localPosition;
                localPosition = Vector3.Lerp(new Vector3(localPosition.x, localPosition.y, z)
                , new Vector3(0,0, z), 3*Time.deltaTime);
                transformItem.localPosition = localPosition;
                await UniTask.NextFrame();
            }

            transformItem.localPosition = new Vector3(0, 0, z);

            _dragHandlers.Dequeue();
        }
    }
}