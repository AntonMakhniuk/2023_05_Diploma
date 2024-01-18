using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.CoreSegmentation
{
    public class GenCoreSegmentation : GeneralBase<CoreSegmentationConfig>
    {
        private CoreSegment[] _allSegments;
        private List<CoreSegment> _stableSegments;
        private List<CoreSegment> _unstableSegments;

        protected override void Start()
        {
            base.Start();
            
            _stableSegments = new List<CoreSegment>(_allSegments);
            _unstableSegments = new List<CoreSegment>();

            foreach (var segment in _allSegments)
            {
                segment.OnSegmentOut += () =>
                {
                    MarkSegmentAsUnstable(segment);
                };
                
                segment.OnSegmentPutBack += () =>
                {
                    MarkSegmentAsStable(segment);
                };
            }
        }
        
        private bool _disablingIsOnCooldown;
        
        protected override void HandleUpdateLogic()
        {
            if (!_disablingIsOnCooldown)
            {
                StartCoroutine(DestabilizeSegments());
            }
        }

        protected override bool CheckFailConditions()
        {
            return _unstableSegments.Count >= Config.failThreshold;
        }

        // TODO: Implement actual logic for checking whether a segment is within warning range
        
        protected override bool CheckWarningConditions()
        {
            return _unstableSegments.Count >= Config.warningThreshold;
        }

        private IEnumerator DestabilizeSegments()
        {
            _disablingIsOnCooldown = true;
            
            int numOfDestabilizations = Random.Range(Config.minDestabilizationCount, Config.maxDestabilizationCount);
                
            for (int i = 0; i < numOfDestabilizations; i++)
            {
                var nextSegment = _stableSegments[Random.Range(0, _allSegments.Length)];
                StartCoroutine(nextSegment.MoveOut());
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
        
        protected override void HandleResetLogic()
        {
            throw new NotImplementedException();
        }

        private void MarkSegmentAsUnstable(CoreSegment unstableSegment)
        {
            _stableSegments.Remove(unstableSegment);
            _unstableSegments.Add(unstableSegment);
        }
        
        private void MarkSegmentAsStable(CoreSegment stableSegment)
        {
            _stableSegments.Add(stableSegment);
            _unstableSegments.Remove(stableSegment);
        }

        // TODO: Implement the two circles
        
        private void InstantiateWarningAndFailCircles()
        {
            
        }
    }

    [Serializable]
    public class CoreSegmentationConfig : GeneralConfigBase
    {
        public float minDisableCooldown;
        public float maxDisableCooldown;
        public int minDestabilizationCount;
        public int maxDestabilizationCount;
        public float warningCircleRadius;
        public float failCircleRadius;
    }
}