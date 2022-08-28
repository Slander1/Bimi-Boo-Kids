using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ItemControllers
{
    public class ItemAnimatedMover : MonoBehaviour
    {
        [SerializeField] private AnimationCurve moveCurve;
                
        private ItemParentSetter _itemParentSetter;
        private readonly Queue<Transform> _dragHandlers = new(); 
        
        public void Init(ItemParentSetter itemParentSetter)
        {
            _itemParentSetter = itemParentSetter;
            Subscribe();
        }
        
        private void Subscribe()
        {
            _itemParentSetter.ParentSet += OnSlotParentSet;
            _itemParentSetter.ItemParentSet += OnItemParentSet;
        }

        private async void OnSlotParentSet(Transform slotTransform)
        {
            await OnParentSet(slotTransform);
        }

        private async void OnItemParentSet(DragHandler dragHandler)
        {
            
            dragHandler.enabled = false;
            await OnParentSet(dragHandler.transform);
            dragHandler.enabled = true;
            var pos = dragHandler.transform.position;
            dragHandler.transform.position = new Vector3(pos.x, pos.y, -4);
        }

        private async UniTask OnParentSet(Transform transformItem)
        {
            
            _dragHandlers.Enqueue(transformItem);
            
            await UniTask.WaitWhile(() => _dragHandlers.Peek() != transformItem);
            
            while (( transformItem.localPosition.x > 0.01f) &&
                   ( transformItem.localPosition.y > 0.01f))
            {
                var localPosition = transformItem.localPosition;
                localPosition = Vector3.Lerp(new Vector3(localPosition.x, localPosition.y, -2)
                , new Vector3(0,0, -2), 3*Time.deltaTime);
                transformItem.localPosition = localPosition;
                await UniTask.NextFrame();
            }

            _dragHandlers.Dequeue();
        }
    }
}