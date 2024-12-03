using NaughtyAttributes;
using Systems.Mining.Tools.Base_Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Ship.Tools.Marker
{
    public class Marker : BaseTool
    {
        [Foldout("Test Bounds Data")] [SerializeField] 
        private GameObject testBoundsObject;
        
        [Foldout("Actual Bounds Data")] [SerializeField] 
        private GameObject actualBoundsPrefab;
        
        [Foldout("Bounds Data")] [SerializeField] 
        private float defaultRadius = 10; 
        [Foldout("Bounds Data")] [SerializeField] 
        private float minRadius = 3;
        [Foldout("Bounds Data")] [SerializeField] 
        private float maxRadius = 20;
        [Foldout("Bounds Data")] [SerializeField] 
        private float radiusChangeSpeed = 20;

        [Foldout("Tool Data")] [SerializeField]
        private float maxRange = 100;
        [Foldout("Tool Data")] [SerializeField]
        private RectTransform crosshairPos;
        
        private float _scanRadius;
        private bool _testBoundsOn;
        private RaycastHit _hitData;
        private TestMarkerBounds _testBounds;

        protected override void Start()
        {
            _scanRadius = defaultRadius;
            
            base.Start();
        }

        protected override void WorkCycle()
        {
            var screenRay = Camera.main.ScreenPointToRay(crosshairPos.position);
            
            var hasHit = Physics.Raycast(screenRay, out var hit, maxRange, 
                ~LayerMask.GetMask("Player", "TransparentFX"));

            _hitData = hit;
            
            if (hasHit && !_testBoundsOn)
            {
                TurnOnTestBounds();
            }
            else if (hasHit)
            {
                UpdateTestBounds();
            }
            else if (_testBoundsOn)
            {
                TurnOffTestBounds();
            }
            
            base.WorkCycle();
        }

        private void TurnOnTestBounds()
        {
            testBoundsObject.SetActive(true);
            UpdateTestBounds();
        }
        
        private void TurnOffTestBounds()
        {
            testBoundsObject.SetActive(false);
        }

        private void UpdateTestBounds()
        {
            testBoundsObject.transform.position = _hitData.point;
            testBoundsObject.transform.localScale = new Vector3(_scanRadius, _scanRadius, _scanRadius);
        }

        protected override void PrimaryActionStarted()
        {
            if (!_testBounds.enabled || !_testBounds.IsValid)
            {
                return;
            }

            var boundsObj = Instantiate(actualBoundsPrefab, _hitData.point, Quaternion.identity, transform);
            var boundsComp = boundsObj.GetComponent<MarkerBounds>();
            
            boundsComp.StartBounds(_scanRadius, radiusChangeSpeed * _scanRadius / maxRadius);
        }

        protected override void PrimaryActionPerformed()
        {
            
        }

        protected override void PrimaryActionCanceled()
        {
            
        }

        protected override void SecondaryActionStarted()
        {
            
        }

        protected override void SecondaryActionPerformed()
        {
            
        }

        protected override void SecondaryActionCanceled()
        {
            
        }

        protected override void ThirdActionStarted()
        {
            
        }

        protected override void ThirdActionPerformed()
        {
            
        }

        protected override void ThirdActionCanceled()
        {
            
        }

        protected override void ScrollPerformed(InputAction.CallbackContext ctx)
        {
            base.ScrollPerformed(ctx);
            
            _scanRadius += ctx.ReadValue<float>() * radiusChangeSpeed * Time.deltaTime;
            _scanRadius = Mathf.Clamp(_scanRadius, minRadius, maxRadius);
        }
    }
}