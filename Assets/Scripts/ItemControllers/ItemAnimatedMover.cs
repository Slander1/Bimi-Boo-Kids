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

        private bool _isDragging = false;

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
            _itemParentSetter.ItemParentSet += OnItemParentSetOrItemNotSuccessed;
            _itemsPositioner.ItemNotSuccessed += OnItemParentSetOrItemNotSuccessed;
        }
        
        private void OnDisable()
        {
            _itemParentSetter.ParentSet -= OnSlotParentSet;
            _itemParentSetter.ItemParentSet -= OnItemParentSetOrItemNotSuccessed;
            _itemsPositioner.ItemNotSuccessed -= OnItemParentSetOrItemNotSuccessed;
        }
        

        private async void OnSlotParentSet(Transform slotTransform)
        {
            await MoveToPosition(slotTransform);
        }

        private async void OnItemParentSetOrItemNotSuccessed(DragHandler dragHandler,  bool isAppear)
        {
            dragHandler.BeginningDragging += OnBeginningDragging;
            await MoveToPosition(dragHandler.transform, true, isAppear);
            dragHandler.BeginningDragging -= OnBeginningDragging;
            _isDragging = false;
        }

        private void OnBeginningDragging(DragHandler obj)
        {
            _isDragging = true;
        }

        private async UniTask MoveToPosition(Transform transformItem, bool isItem = false, bool isAppear = true)
        {
            _dragHandlers.Enqueue(transformItem);
            
            await UniTask.WaitWhile(() => _dragHandlers.Peek() != transformItem);
            
            if (isAppear)
                ItemAppeared?.Invoke();
            
            var z = (isItem) ? -4 : -2;
            var i = 1;
            while (( transformItem.localPosition.x > 0.5f) &&
                   ( transformItem.localPosition.y > 0.5f))
            {
                var localPosition = transformItem.localPosition;
                localPosition = Vector3.Lerp(new Vector3(localPosition.x, localPosition.y, z)
                , new Vector3(0,0, z), moveCurve.Evaluate(i) *Time.deltaTime);
                transformItem.localPosition = localPosition;
                await UniTask.NextFrame();
                i++;
                
                if (_isDragging)
                {
                    _dragHandlers.Dequeue();
                    return;
                }
            }

            transformItem.localPosition = new Vector3(0, 0, z);

            _dragHandlers.Dequeue();
        }
    }
}