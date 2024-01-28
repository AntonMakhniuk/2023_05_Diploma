using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Core_Segmentation
{
    public class GenCoreSegmentation : GeneralBase<CoreSegmentationConfig>
    {
        [SerializeField] private GameObject segmentPrefab;
        
        public ColliderRendererCommunicator warningCollider;
        public ColliderRendererCommunicator failCollider;
        
        private List<CoreSegment> _allSegments;
        private List<CoreSegment> _stableSegments;
        private List<CoreSegment> _unstableSegments;
        private List<CoreSegment> _segmentsInWarningZone;
        private bool _disablingIsOnCooldown;

        protected override void Start()
        {
            base.Start();
            
            _allSegments = new List<CoreSegment>();
            _unstableSegments = new List<CoreSegment>();
            _segmentsInWarningZone = new List<CoreSegment>();

            PrepareColliders();
            InstantiateSegments();
            
            foreach (var segment in _allSegments)
            {
                segment.OnSegmentInFailZone += MarkSegmentAsUnstable;
                segment.OnSegmentStabilized += MarkSegmentAsStable;
                segment.OnSegmentInWarningZone += MarkSegmentAsInWarning;
                segment.OnSegmentInSafeZone += RemoveInWarningMarking;
            }

            _stableSegments = new List<CoreSegment>(_allSegments);
            
            // Added so that segments don't get destabilized as soon as the challenge starts
            StartCoroutine(DelaySegmentDestabilization(Config.maxDisableCooldown));
        }
        
        private void MarkSegmentAsUnstable(CoreSegment unstableSegment)
        {
            if (!_stableSegments.Contains(unstableSegment) || _unstableSegments.Contains(unstableSegment))
            {
                return;
            }
            
            _stableSegments.Remove(unstableSegment);
            _unstableSegments.Add(unstableSegment);
            
            Debug.Log("Added to unstable, left " + _unstableSegments.Count);
            Debug.Log("Removed from stable, left " + _stableSegments.Count);
        }
        
        private void MarkSegmentAsStable(CoreSegment stableSegment)
        {
            if (_stableSegments.Contains(stableSegment) || !_unstableSegments.Contains(stableSegment))
            {
                return;
            }
            
            _stableSegments.Add(stableSegment);
            _unstableSegments.Remove(stableSegment);
            
            Debug.Log("Removed from unstable, left " + _unstableSegments.Count);
            Debug.Log("Added to stable, left " + _stableSegments.Count);
        }

        private void MarkSegmentAsInWarning(CoreSegment segmentInWarning)
        {
            if (_segmentsInWarningZone.Contains(segmentInWarning))
            {
                return;
            }
            
            _segmentsInWarningZone.Add(segmentInWarning);
            
            Debug.Log("Added to warning, left " + _segmentsInWarningZone.Count);
        }

        private void RemoveInWarningMarking(CoreSegment safeSegment)
        {
            if (!_segmentsInWarningZone.Contains(safeSegment))
            {
               return;
            }
            
            _segmentsInWarningZone.Remove(safeSegment);
            
            Debug.Log("Removed from warning, left " + _segmentsInWarningZone.Count);
        }
        
        private IEnumerator DelaySegmentDestabilization(float timeInSeconds)
        {
            _disablingIsOnCooldown = true;
            
            yield return new WaitForSeconds(timeInSeconds);

            _disablingIsOnCooldown = false;

            yield return null;
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

        private bool _isResetting;
        
        protected override bool CheckWarningConditions()
        {
            if (_isResetting)
            {
                return false;
            }
            
            return _segmentsInWarningZone.Count >= Config.warningThreshold;
        }
        
        protected override bool CheckFailConditions()
        {
            if (_isResetting)
            {
                return false;
            }
            
            return _unstableSegments.Count >= Config.failThreshold;
        }
        
        protected override void HandleUpdateLogic()
        {
            if (_disablingIsOnCooldown || _isResetting)
            {
                return;
            }
            
            StartCoroutine(DestabilizeSegments());
        }

        private IEnumerator DestabilizeSegments()
        {
            _disablingIsOnCooldown = true;
            
            int numOfDestabilizations = Random.Range(Config.minDestabilizationCount, Config.maxDestabilizationCount);
            
            for (int i = 0; i < numOfDestabilizations; i++)
            {
                var nextSegment = _stableSegments[Random.Range(0, _stableSegments.Count)];
                nextSegment.MoveOut(Config.destabilizationTravelTime);
            }
            
            yield return new WaitForSeconds(Random.Range(Config.minDisableCooldown, Config.maxDisableCooldown));
            
            _disablingIsOnCooldown = false;

            yield return null;
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
            _isResetting = true;
            
            StopAllCoroutines();

            foreach (var segment in _allSegments)
            {
                StartCoroutine(segment.MoveOutAndBackForReset());

                yield return null;
            }
            
            _unstableSegments.Clear();
            _stableSegments = new List<CoreSegment>(_allSegments);

            yield return new WaitForSeconds(Config.resetWaitingTime);

            _isResetting = false;

            yield return null;
        }
        
        private void PrepareColliders()
        {
            warningCollider.DrawCollider();
            failCollider.DrawCollider();
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
    }
}