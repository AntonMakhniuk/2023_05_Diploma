using NaughtyAttributes;
using Scriptable_Object_Templates.Resources;
using UnityEngine;

namespace Player.Ship.Tools.Marker
{
    public class Collectable : MonoBehaviour
    {
        [Foldout("Resource Data")]
        public ResourceData resource;
        [Foldout("Resource Data")] [SerializeField] 
        private bool isScaleDependent;

        [Foldout("Visualisation Data")] [SerializeField]
        private GameObject associatedMarker;
        
        private float _amountContained;
        public float AmountContained
        {
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _amountContained = value;
            }
            get => isScaleDependent ? _amountContained * transform.localScale.magnitude : _amountContained;
        }

        [ReadOnly] public bool isMarked;

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
}