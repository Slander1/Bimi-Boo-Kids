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
        private Queue<Transform> _dragHandlers = new(); 
        
        public void Init(ItemParentSetter itemParentSetter)
        {
            _itemParentSetter = itemParentSetter;
            Subscribe();
        }
        
        private void Subscribe()
        {
            _itemParentSetter.ParentSet += OnParentSet;
        }

        private async void OnParentSet(Transform transformItem, int z)
        {
            _dragHandlers.Enqueue(transformItem);
            
            await UniTask.WaitWhile(() => _dragHandlers.Peek() != transformItem);
            var i = 1;
            while (( transformItem.localPosition.x > 0.01f) &&
                ( transformItem.localPosition.y > 0.01f))
            {
                var localPosition = transformItem.localPosition;
                localPosition = Vector3.Lerp(new Vector3(localPosition.x, localPosition.y, -2)
                , new Vector3(0,0, z), 3*Time.deltaTime);
                transformItem.localPosition = localPosition;
                i++;
                await UniTask.NextFrame();
            }

            _dragHandlers.Dequeue();
        }
    }
}