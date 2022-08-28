using System.Collections.Generic;
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

        private async void OnParentSet(Transform pos)
        {
            _dragHandlers.Enqueue(pos);

            

            _dragHandlers.Dequeue();
        }
    }
}