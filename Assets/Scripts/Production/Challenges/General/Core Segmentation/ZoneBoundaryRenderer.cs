using System;
using ThirdParty.Scripts;
using UnityEngine;

namespace Production.Challenges.General.Core_Segmentation
{
    public class ZoneBoundaryRenderer : MonoBehaviour
    {
        public GenCoreSegmentation segmentationChallenge;
        public UILineRenderer uiLineRenderer;
        public int stepCount;
        
        [SerializeField] private ZoneType zoneType;
        
        private void Start()
        {
            if (segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated without GenSegmentationChallenge");
            }
            
            if (uiLineRenderer == null)
            {
                throw new Exception($"'{nameof(uiLineRenderer)}' is null. " +
                                    $"{GetType().Name} has been instantiated without UILineRenderer");
            }

            switch (zoneType)
            {
                case ZoneType.Warning:
                {
                    GeneratePoints(segmentationChallenge.Config.warningZoneRadius);

                    break;
                }
                case ZoneType.Fail:
                {
                    GeneratePoints(segmentationChallenge.failZoneRadius);
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GeneratePoints(float radius)
        {
            uiLineRenderer.points = new Vector2[stepCount + 1];

            float angleStep = 2f * Mathf.PI / stepCount;
            
            for (int i = 0; i <= stepCount; i++)
            {
                float xPosition = radius * Mathf.Cos(angleStep * i);
                float yPosition = radius * Mathf.Sin(angleStep * i);

                Vector2 circlePoint = new Vector2(xPosition, yPosition);

                uiLineRenderer.points[i] = circlePoint;
            }
        }
    }

    enum ZoneType
    {
        Warning, Fail
    }
}