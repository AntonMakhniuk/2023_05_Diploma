using System;
using UnityEngine;
using UnityEngine.UI;

namespace Production.Challenges.General.Core_Segmentation
{
    // Needed to associate an actual running instance with the button
    public class StabilizerAccessButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private GenCoreSegmentation _segmentationChallenge;
        private Stabilizer _stabilizer;

        private void Start()
        {
            _segmentationChallenge = GetComponentInParent<GenCoreSegmentation>();

            if (_segmentationChallenge == null)
            {
                throw new Exception($"'{nameof(_segmentationChallenge)}' is null. " +
                                    $"{GetType().Name} has been instantiated outside GenSegmentationChallenge");
            }
            
            button.onClick.AddListener(_segmentationChallenge.stabilizer.FireStabilizer);
        }
    }
}
