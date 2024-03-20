using System;
using System.Collections;
using Miscellaneous;
using Production.Crafting;
using Production.Systems;
using ThirdParty.Scripts;
using UnityEngine;

namespace Production.Challenges.General.Core_Segmentation
{
    public class StabilizerLineRenderer : MonoBehaviour
    {
        public Stabilizer stabilizer;
        public UILineRenderer uiLineRenderer;

        private GenCoreSegmentation _segmentationChallenge;
        private float _stabilizerOrbitRadius;

        private void Start()
        {
            _segmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(_segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenSegmentationChallenge");
            }
            
            if (stabilizer == null)
            {
                throw new Exception($"'{nameof(stabilizer)}' is null. " +
                                    $"{GetType().Name} has been instantiated without Stabilizer");
            }

            if (uiLineRenderer == null)
            {
                throw new Exception($"'{nameof(uiLineRenderer)}' is null. " +
                                    $"{GetType().Name} has been instantiated without UILineRenderer");
            }

            _stabilizerOrbitRadius = _segmentationChallenge.stabilizerOrbitRadius;
            
            GenerateLine();

            stabilizer.OnStabilizerTrajectoryUpdated += UpdateLine;
            _segmentationChallenge.OnGeneralFail += HandleChallengeFail;
            _segmentationChallenge.OnGeneralReset += HandleChallengeReset;
            ProductionManager.Instance.currentManager.OnProductionFinished += HandleProductionEnd;
        }

        private void GenerateLine()
        {
            uiLineRenderer.points = new Vector2[2];

            uiLineRenderer.points[0] = stabilizer.edgePointTransform.localPosition;
        }

        public Color32 pointingAtSegmentLineColor, pointingAtCenterLineColor;

        private bool _challengeHasFailed, _rayIsMovingBack;
        private RaycastHit2D _currentHit;
        
        private void UpdateLine(RaycastHit2D hit, bool isPointingAtSegment)
        {
            _currentHit = hit;
            
            if (_challengeHasFailed || _rayIsMovingBack)
            {
                return;
            }
            
            float lineLengthScaled = Vector2.Distance(stabilizer.edgePointTransform.position, hit.point)
                / ProductionManager.Instance.transform.localScale.x;
            
            if (isPointingAtSegment)
            {
                // The x is added to offset the ray in length enough to where it hugs the segments
                uiLineRenderer.points[1].x = lineLengthScaled + stabilizer.edgePointTransform.localPosition.x;
            }
            else
            {
                uiLineRenderer.points[1].x = _stabilizerOrbitRadius;
            }
                
            uiLineRenderer.color = !isPointingAtSegment ? pointingAtCenterLineColor : pointingAtSegmentLineColor;
            
            uiLineRenderer.ForceRedraw();
        }

        [SerializeField] private AnimationCurve retractCurve, moveBackCurve;
        
        private void HandleChallengeFail()
        {
            _challengeHasFailed = true;
            
            uiLineRenderer.color = pointingAtCenterLineColor;
            
            StartCoroutine(MoveRayCoroutine(false, uiLineRenderer.points[1].x, 
                uiLineRenderer.points[0].x, retractCurve, stabilizer.rayMovementTime));
        }

        private void HandleProductionEnd(object sender, CraftingData craftingData)
        {
            _segmentationChallenge.OnGeneralFail -= HandleChallengeFail;
            _segmentationChallenge.OnGeneralReset -= HandleChallengeReset;
            
            HandleChallengeFail();
        }
        
        private IEnumerator MoveRayCoroutine(bool isReturning, float startPosition, 
            float endPosition, AnimationCurve stabilizerCurve, float timeToMoveBack)
        {
            IEnumerator currentEnumerator = Utility.LerpFloat
            (
                startPosition,
                endPosition,
                stabilizerCurve,
                timeToMoveBack
            );
            
            while (currentEnumerator.MoveNext())
            {
                uiLineRenderer.points[1].x = (float) currentEnumerator.Current!;
                
                uiLineRenderer.ForceRedraw();
                
                if (isReturning)
                {
                    float lineLengthScaledWithEdgePoint = 
                        Vector2.Distance(stabilizer.edgePointTransform.position, _currentHit.point)
                        / ProductionManager.Instance.transform.localScale.x 
                        + stabilizer.edgePointTransform.localPosition.x;

                    if (lineLengthScaledWithEdgePoint < uiLineRenderer.points[1].x)
                    {
                        uiLineRenderer.points[1].x = lineLengthScaledWithEdgePoint;

                        _rayIsMovingBack = false;
                        
                        uiLineRenderer.ForceRedraw();
                        
                        yield break;
                    }
                }
                
                yield return null;
            }

            _rayIsMovingBack = false;
        }
        
        private void HandleChallengeReset(object sender, EventArgs args)
        {
            _challengeHasFailed = false;
            
            StartCoroutine(RestartRayCoroutine());
        }

        
        private IEnumerator RestartRayCoroutine()
        {
            _rayIsMovingBack = true;
            
            float lineLengthScaledWithEdgePoint = 
                Vector2.Distance(stabilizer.edgePointTransform.position, _currentHit.point)
                / ProductionManager.Instance.transform.localScale.x 
                + stabilizer.edgePointTransform.localPosition.x;
            
            yield return StartCoroutine(MoveRayCoroutine(true, uiLineRenderer.points[1].x,
                uiLineRenderer.points[1].x + lineLengthScaledWithEdgePoint, moveBackCurve, 
                stabilizer.rayMovementTime));
        }
    }
}