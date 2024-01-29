using UnityEngine;

namespace Production.Challenges.General.Core_Segmentation
{
    public class ColliderRendererCommunicator : MonoBehaviour
    {
        public CircleCollider2D segmentationCircleCollider;
        public LineRenderer colliderRenderer;
        public int circleRenderStepCount = 50;
        
        // TODO: Implement logic to change draw settings based on collider type
        
        public void DrawCollider()
        {
            colliderRenderer.positionCount = circleRenderStepCount;
 
            for(int currentStep = 0; currentStep < circleRenderStepCount; currentStep++)
            {
                float circumferenceProgress = (float) currentStep / (circleRenderStepCount - 1);
 
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            
                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                var radius = segmentationCircleCollider.radius;
                
                float x = radius * xScaled;
                float y = radius * yScaled;
                float z = 0;
 
                Vector3 currentPosition = new Vector3(x,y,z);
 
                colliderRenderer.SetPosition(currentStep, currentPosition);
            }
        }
    }
}