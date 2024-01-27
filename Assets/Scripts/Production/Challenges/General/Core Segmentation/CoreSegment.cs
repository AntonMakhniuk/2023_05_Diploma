using System;
using System.Collections;
using UnityEngine;

namespace Production.Challenges.General.Core_Segmentation
{
    public class CoreSegment : MonoBehaviour
    {
        public delegate void SegmentOutHandler();
        public event SegmentOutHandler OnSegmentOut;
        
        public delegate void SegmentInHandler();
        public event SegmentInHandler OnSegmentPutBack;

        private GenCoreSegmentation _parentSegmentationChallenge;
        private CoreSegmentationConfig _config;

        private void Start()
        {
            _parentSegmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_parentSegmentationChallenge == null)
            {
                throw new Exception($"{this} has been instantiated outside GenCoreSegmentation");
            }

            _config = _parentSegmentationChallenge.Config;
        }

        public IEnumerator MoveOut()
        {
            // TODO: Implement MoveOut logic

            OnSegmentOut?.Invoke();
            
            yield return null;
        }

        public IEnumerator PutBack()
        {
            // TODO: Implement PutBack logic
            
            OnSegmentPutBack?.Invoke();

            yield return null;
        }
    }
}