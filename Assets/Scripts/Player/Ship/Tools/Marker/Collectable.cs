using System;
using NaughtyAttributes;
using Scriptable_Object_Templates.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Ship.Tools.Marker
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
        public bool isMarked;
        [Foldout("Debug Data")] [SerializeField] [ReadOnly] 
        private float amountContained;

        public float AmountContained => amountContained;

        private void Awake()
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

        public void TurnOnMark()
        {
            isMarked = true;
            
            associatedMarker.SetActive(isMarked);
        }
        
        public void TurnOffMark()
        {
            isMarked = false;
            
            associatedMarker.SetActive(false);
        }
    }

    internal enum ResourceContainerType
    {
        Constant,
        Range
    }
}