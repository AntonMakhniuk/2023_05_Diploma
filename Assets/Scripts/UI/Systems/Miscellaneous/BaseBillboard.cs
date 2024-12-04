using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Testing.NewMovement;
using UnityEngine;

namespace UI.Systems.Miscellaneous
{
    public class BaseBillboard : MonoBehaviour
    {
        [Foldout("Billboard Data")] [SerializeField]
        private float scaleMultiplier = 5f;
        [Foldout("Billboard Data")] [SerializeField]
        private float fadeStartDistance = 300f;
        [Foldout("Billboard Data")] [SerializeField]
        private float fadeEndDistance = 500f;
        [Foldout("Billboard Data")] [SerializeField]
        private float distanceScaleMultiplier = 5f;
        [Foldout("Billboard Data")] [SerializeField]
        private float scaleStartDistance = 100f;
        [Foldout("Billboard Data")] [SerializeField]
        private CanvasGroup canvasGroup;
        
        protected float DistanceFromPlayer;
        
        private Vector3 _originalScale;
        private List<SpriteRenderer> _sprites;

        private void Awake()
        {
            var parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
            var inverseParentScale = 
                new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);
            
            transform.localScale = Vector3.Scale(transform.localScale, inverseParentScale) * scaleMultiplier;
            _originalScale = transform.localScale;
            _sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        protected virtual void LateUpdate()
        {
            transform.LookAt(Camera.main!.transform.position, Camera.main!.transform.up);
            
            DistanceFromPlayer =
                Vector3.Distance(transform.position, DroneController.Instance.gameObject.transform.position);

            if (DistanceFromPlayer >= fadeEndDistance)
            {
                canvasGroup.alpha = 0f;

                foreach (var sprite in _sprites)
                {
                    var color = sprite.color;
                    color.a = 0f;
                    sprite.color = color;
                }
            }
            else if (DistanceFromPlayer >= fadeStartDistance)
            {
                canvasGroup.alpha = fadeEndDistance - DistanceFromPlayer / fadeEndDistance - fadeStartDistance;
                
                foreach (var sprite in _sprites)
                {
                    var color = sprite.color;
                    color.a = fadeEndDistance - DistanceFromPlayer / fadeEndDistance - fadeStartDistance;
                    sprite.color = color;
                }
            }
            else
            {
                canvasGroup.alpha = 1f;
                
                foreach (var sprite in _sprites)
                {
                    var color = sprite.color;
                    color.a = 1f;
                    sprite.color = color;
                }
            }

            var parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
            var inverseParentScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);
            
            Debug.Log("dist: " + DistanceFromPlayer);
            
            if (DistanceFromPlayer > scaleStartDistance)
            {
                var normalizedDistance = 
                    Mathf.InverseLerp(scaleStartDistance, fadeEndDistance, DistanceFromPlayer);
                Debug.Log("normDist: " + normalizedDistance);
                
                var scaleFactor = Mathf.Lerp(_originalScale.x, distanceScaleMultiplier, normalizedDistance);
                Debug.Log("scale: " + scaleFactor);
                
                transform.localScale = Vector3.Scale(_originalScale * scaleFactor, inverseParentScale);
            }
            else
            {
                transform.localScale = Vector3.Scale(_originalScale, inverseParentScale);
            }
        }
    }
}