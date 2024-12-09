using System;
using NaughtyAttributes;
using Scriptable_Object_Templates.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Inventory
{
    public class Collectable : MonoBehaviour
    {
        [Foldout("Collectable Data")]
        public ResourceData resource;

        [Foldout("Resource Data")] [SerializeField]
        private ResourceContainerType containerType;
        [Foldout("Resource Data")] [SerializeField] 
        [ShowIf("containerType", ResourceContainerType.Constant)] 
        private float resourceAmountConstant;
        [Foldout("Resource Data")] [SerializeField] [MinMaxSlider(0f, 100f)]
        [ShowIf("containerType", ResourceContainerType.Range)] 
        private Vector2 resourceAmountRange;
        [Foldout("Resource Data")] [SerializeField] 
        private bool isScaleDependent;

        [Foldout("Visualisation Data")] [SerializeField]
        private GameObject associatedMarker;

        [Foldout("Debug Data")] [ReadOnly] 
        public CollectableState state;
        [Foldout("Debug Data")] [SerializeField] [ReadOnly] 
        private float amountContained;

        public float AmountContained => amountContained;

        private void Start()
        {
            amountContained = containerType switch
            {
                ResourceContainerType.Constant => resourceAmountConstant,
                ResourceContainerType.Range => Random.Range(resourceAmountRange.x, resourceAmountRange.y),
                _ => amountContained
            };
            
            if (isScaleDependent)
            {
                var scale = transform.lossyScale;

                amountContained *= (scale.x + scale.y + scale.z) / 3;
            }
            
            amountContained = (float)Math.Round(amountContained, 2);
        }

        public static event EventHandler<Collectable> OnCollectableMarked; 

        public void TurnOnMark()
        {
            state = CollectableState.Marked;
            
            OnCollectableMarked?.Invoke(this, this);
            
            associatedMarker.SetActive(true);
        }
        
        public void TurnOffMark()
        {
            state = CollectableState.Unmarked;
            
            associatedMarker.SetActive(false);
        }
    }

    internal enum ResourceContainerType
    {
        Constant,
        Range
    }

    public enum CollectableState
    {
        Unmarked,
        Marked,
        InQueue,
        InTransit
    }
}