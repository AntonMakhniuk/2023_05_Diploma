using System;
using System.Collections;
using System.Collections.Generic;
using Production.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Core_Segmentation
{
    public class GenCoreSegmentation : GeneralBase<CoreSegmentationConfig>
    {
        public float failZoneRadius;
        
        [HideInInspector] public float warningZoneRadiusScaled;
        [HideInInspector] public float failZoneRadiusScaled;
        
        [SerializeField] private GameObject segmentPrefab;
        
        private List<CoreSegment> _allSegments;
        private List<CoreSegment> _stableSegments;
        private List<CoreSegment> _unstableSegments;
        private List<CoreSegment> _warningSegments;

        protected override void Start()
        {
            base.Start();
            
            // TODO: Unexpected behaviour WILL happen if the aspect ratio is changed mid-production, as its Start-only
            var managerScale = ProductionManager.Instance.transform.localScale;
            warningZoneRadiusScaled = Config.warningZoneRadius * managerScale.x;
            failZoneRadiusScaled = failZoneRadius * managerScale.x;
            
            _allSegments = new List<CoreSegment>();
            _unstableSegments = new List<CoreSegment>();
            _warningSegments = new List<CoreSegment>();
            
            InstantiateSegments();
            
            foreach (var segment in _allSegments)
            {
                segment.OnSegmentEnteredFailZone += MarkSegmentAsUnstable;
                segment.OnSegmentEnteredWarningZone += MarkSegmentAsWarning;
                segment.OnSegmentEnteredSafeZone += MarkSegmentAsStable;
            }

            _stableSegments = new List<CoreSegment>(_allSegments);
            
            // Added so that segments don't get destabilized as soon as the challenge starts
            StartCoroutine(PauseUpdateForSeconds(Config.maxDisableCooldown));
        }
        
        private void MarkSegmentAsUnstable(object sender, CoreSegment segment)
        {
            _stableSegments.Remove(segment);
            _warningSegments.Remove(segment);
            _unstableSegments.Add(segment);
        }
        
        private void MarkSegmentAsStable(object sender, CoreSegment segment)
        {
            _stableSegments.Add(segment);
            _warningSegments.Remove(segment);
            _unstableSegments.Remove(segment);
        }

        private void MarkSegmentAsWarning(object sender, CoreSegment segment)
        {
            _stableSegments.Remove(segment);
            _warningSegments.Add(segment);
            _unstableSegments.Remove(segment);
        }
        
        private void InstantiateSegments()
        {
            var parentTransform = transform;
            var rotation = segmentPrefab.transform.rotation;
            
            for (int i = 0; i < Config.numOfSegments; i++)
            {
                var newSegment = Instantiate
                (
                    segmentPrefab,
                    parentTransform.position,
                    rotation * Quaternion.Euler(0, 0, 360f / Config.numOfSegments * i),
                    parentTransform
                );
                
                _allSegments.Add(newSegment.GetComponent<CoreSegment>());
            }
        }
        
        protected override bool CheckWarningConditions()
        {
            return _warningSegments.Count >= Config.warningThreshold;
        }
        
        protected override bool CheckFailConditions()
        {
            return _unstableSegments.Count >= Config.failThreshold;
        }
        
        protected override void HandleUpdateLogic()
        {
            StartCoroutine(DestabilizeSegments());
        }

        private IEnumerator DestabilizeSegments()
        {
            UpdateLogicIsPaused = true;
            
            int numOfDestabilizations = Random.Range(Config.minDestabilizationCount, Config.maxDestabilizationCount);
            
            for (int i = 0; i < numOfDestabilizations; i++)
            {
                var nextSegment = _stableSegments[Random.Range(0, _stableSegments.Count)];
                nextSegment.InterruptAndMoveOut(Config.destabilizationTravelTime);
            }
            
            yield return new WaitForSeconds(Random.Range(Config.minDisableCooldown, Config.maxDisableCooldown));
            
            UpdateLogicIsPaused = false;
        }
        
        public event EventHandler OnCoreSegmentationAboveWarningThreshold;
        
        protected override void HandleWarningStart()
        {
            OnCoreSegmentationAboveWarningThreshold?.Invoke(this, null);
        }

        public event EventHandler OnCoreSegmentationBelowWarningThreshold;
        
        protected override void HandleWarningStop()
        {
            OnCoreSegmentationBelowWarningThreshold?.Invoke(this, null);
        }
        
        protected override IEnumerator HandleResetLogic()
        {
            UpdateLogicIsPaused = false;
            
            StopAllCoroutines();
            
            foreach (var segment in _allSegments)
            {
                yield return StartCoroutine(segment.MoveOutAndBackForReset());
            }

            UpdateLogicIsPaused = true;
        }
    }

    [Serializable]
    public class CoreSegmentationConfig : GeneralConfigBase
    {
        public int numOfSegments;
        public float minDisableCooldown;
        public float maxDisableCooldown;
        public int minDestabilizationCount;
        public int maxDestabilizationCount;
        public float destabilizationTravelTime;
        public float resetTravelTime;
        public float warningZoneRadius;
    }
}