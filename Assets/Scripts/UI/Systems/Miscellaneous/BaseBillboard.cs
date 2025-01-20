using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Player.Movement.Drone_Movement;
using UnityEngine;

namespace UI.Systems.Miscellaneous
{
    public class BaseBillboard : MonoBehaviour
    {
        [Foldout("Billboard Scale Data")] [SerializeField]
        private float scaleMultiplier = 2f;
        [Foldout("Billboard Scale Data")] [SerializeField]
        private float minimumScale = 1f;
        [Foldout("Billboard Scale Data")] [SerializeField]
        private float scalingAssumedDistance = 25f;
        [Foldout("Billboard Scale Data")] [CurveRange(0, 0, 1, 1)] [SerializeField]
        private AnimationCurve scaleDropOffCurve;
        [Foldout("Billboard Fade Data")] [SerializeField]
        private float fadeStartDistance = 300f;
        [Foldout("Billboard Fade Data")] [SerializeField]
        private float fadeEndDistance = 500f;
        [Foldout("Billboard Fade Data")] [SerializeField]
        private float minimumAlpha = 0.1f;
        [Foldout("Billboard Data")] [SerializeField]
        private CanvasGroup canvasGroup;
        
        protected float DistanceFromPlayer;
        
        private Vector3 _originalScale;
        private List<SpriteRenderer> _sprites;
        private Transform _transform;
        private bool _minAlphaSet, _maxAlphaSet;

        private void Awake()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        protected virtual void Start()
        {
            _transform = transform;
            
            var parent = _transform.parent;
            
            _transform.parent = null;
            _transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
            _transform.parent = parent;
            
            _originalScale = _transform.localScale;
        }

        protected virtual void LateUpdate()
        {
            _transform.LookAt(Camera.main!.transform.position, Camera.main!.transform.up);
            
            DistanceFromPlayer =
                Vector3.Distance(_transform.position, DroneController.Instance.gameObject.transform.position);

            if (DistanceFromPlayer >= fadeEndDistance)
            {
                if (!_minAlphaSet)
                {
                    canvasGroup.alpha = minimumAlpha;
                
                    foreach (var sprite in _sprites)
                    {
                        var color = sprite.color;
                        color.a = minimumAlpha;
                        sprite.color = color;
                    }

                    _minAlphaSet = true;
                }
            }
            else if (DistanceFromPlayer >= fadeStartDistance)
            {
                var alphaT = (fadeEndDistance - DistanceFromPlayer) / (fadeEndDistance - fadeStartDistance);
                var alpha = Mathf.InverseLerp(minimumAlpha, 1f, alphaT);
                
                canvasGroup.alpha = alpha;
                
                foreach (var sprite in _sprites)
                {
                    var color = sprite.color;
                    color.a = alpha;
                    sprite.color = color;
                }

                _minAlphaSet = _maxAlphaSet = false;
            }
            else
            {
                if (!_maxAlphaSet)
                {
                    canvasGroup.alpha = 1f;

                    foreach (var sprite in _sprites)
                    {
                        var color = sprite.color;
                        color.a = 1f;
                        sprite.color = color;
                    }

                    _maxAlphaSet = true;
                }
            }

            var scaleFactor = DistanceFromPlayer / scalingAssumedDistance;
            var curveT = Mathf.InverseLerp(0, fadeEndDistance, DistanceFromPlayer);

            scaleFactor *= scaleDropOffCurve.Evaluate(curveT);
            _transform.localScale = 
                scaleFactor < minimumScale ? _originalScale * minimumScale : _originalScale * scaleFactor;

        }
    }
}