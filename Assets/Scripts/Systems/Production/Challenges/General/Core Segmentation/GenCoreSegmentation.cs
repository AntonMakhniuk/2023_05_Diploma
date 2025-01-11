using System;
using System.Collections;
using System.Collections.Generic;
using Production.Systems;
using Systems.Production.Challenges.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Challenges.General.Core_Segmentation
{
    public class GenCoreSegmentation : GeneralBase<CoreSegmentationConfig>
    {
        public float failZoneRadius;
        public float stabilizerOrbitRadius;
        
        [HideInInspector] public Stabilizer stabilizer;
        [HideInInspector] public float warningZoneRadiusScaled;
        [HideInInspector] public float failZoneRadiusScaled;
        
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private GameObject stabilizerPrefab;
        
        public GameObject childCanvas;
        
        private List<CoreSegment> _allSegments;
        private List<CoreSegment> _stableSegments;
        private List<CoreSegment> _unstableSegments;
        private List<CoreSegment> _warningSegments;

        public override void Setup()
        {
            base.Setup();
            
            // TODO: Unexpected behaviour WILL happen if the aspect ratio is changed mid-production, as its Start-only
            var managerScale = ProductionManager.Instance.transform.localScale;
            warningZoneRadiusScaled = Config.warningZoneRadius * managerScale.x;
            failZoneRadiusScaled = failZoneRadius * managerScale.x;
            
            _allSegments = new List<CoreSegment>();
            _unstableSegments = new List<CoreSegment>();
            _warningSegments = new List<CoreSegment>();
            
            InstantiateElements();
            
            foreach (var segment in _allSegments)
            {
                segment.OnSegmentEnteredFailZone += MarkSegmentAsUnstable;
                segment.OnSegmentEnteredWarningZone += MarkSegmentAsWarning;
                segment.OnSegmentLeftCenter += MarkSegmentAsLeftCenter;
                segment.OnSegmentEnteredCenter += MarkSegmentEnteredCenter;
            }

            _stableSegments = new List<CoreSegment>(_allSegments);
            
            // Added so that segments don't get destabilized as soon as the challenge starts
            StartCoroutine(PauseUpdateForSeconds(Config.maxDisableCooldown));
        }

        protected override void ChangeInteractive(bool newState)
        {
            foreach (var element in interactiveElementsParents)
            {
                element.SetActive(newState);
            }
        }

        private void MarkSegmentAsUnstable(object sender, CoreSegment segment)
        {
            _stableSegments.Remove(segment);
            _warningSegments.Remove(segment);
            _unstableSegments.Add(segment);
        }
        
        private void MarkSegmentAsWarning(object sender, CoreSegment segment)
        {
            _stableSegments.Remove(segment);
            _warningSegments.Add(segment);
            _unstableSegments.Remove(segment);
        }
        
        private void MarkSegmentAsLeftCenter(object sender, CoreSegment segment)
        {
            _stableSegments.Remove(segment);
            _warningSegments.Remove(segment);
            _unstableSegments.Remove(segment);
        }
        
        private void MarkSegmentEnteredCenter(object sender, CoreSegment segment)
        {
            _stableSegments.Add(segment);
            _warningSegments.Remove(segment);
            _unstableSegments.Remove(segment);
        }
        
        private void InstantiateElements()
        {
            var canvasTransform = childCanvas.transform;
            var rotation = segmentPrefab.transform.rotation;
            
            for (int i = 0; i < Config.numOfSegments; i++)
            {
                var newSegment = Instantiate
                (
                    segmentPrefab,
                    canvasTransform.position,
                    rotation * Quaternion.Euler(0, 0, 360f / Config.numOfSegments * i),
                    canvasTransform
                );
                
                _allSegments.Add(newSegment.GetComponent<CoreSegment>());
            }

            var stabilizerObject = Instantiate
            (
                stabilizerPrefab,
                canvasTransform.position + new Vector3(0, stabilizerOrbitRadius, 0),
                rotation,
                canvasTransform
            );

            stabilizer = stabilizerObject.GetComponent<Stabilizer>();
        }
        
        protected override int GetNumberOfWarnings()
        {
            return _warningSegments.Count;
        }
        
        protected override int GetNumberOfFails()
        {
            return _unstableSegments.Count;
        }

        protected override void UpdateChallengeElements()
        {
                StartCoroutine(DestabilizeSegments());
        }

        private IEnumerator DestabilizeSegments()
        {
            UpdateIsPaused = true;
            
            int numOfDestabilizations = Random.Range(Config.minDestabilizationCount, Config.maxDestabilizationCount);
            
            for (int i = 0; i < numOfDestabilizations; i++)
            {
                if (_stableSegments.Count == 0)
                {
                    break;
                }
                
                var nextSegment = _stableSegments[Random.Range(0, _stableSegments.Count)];
                
                _stableSegments.Remove(nextSegment);
                
                nextSegment.InterruptAndMoveOut(Config.destabilizationTravelTime);
            }
            
            yield return new WaitForSeconds(Random.Range(Config.minDisableCooldown, Config.maxDisableCooldown));

            UpdateIsPaused = false;
        }
        
        protected override IEnumerator ResetLogicCoroutine()
        {
            foreach (var segment in _allSegments)
            {
                StartCoroutine(segment.MoveOutAndBack());
            }

            StartCoroutine(stabilizer.SpeedUpAndReset(Config.resetWaitingTime * 2));

            yield return new WaitForSeconds(Config.resetWaitingTime * 2);
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