using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Production.Challenges.General;
using UnityEngine;

namespace Production.Challenges.General.Rod_Positioning
{
    public class GenRodPositioning : GeneralBase<RodPositioningConfig>
    {
        [SerializeField] private List<RodLever> levers = new();
        
        // TODO: implement start method and instantiate levers

        protected override void UpdateChallengeElements()
        {
            foreach (var rodLever in levers)
            {
                rodLever.UpdateCurrentPosition();
            }
        }

        protected override int GetNumberOfWarnings()
        {
            int numOfLeversInWarnRange = 0;

            foreach (var rodLever in levers)
            {
                if (rodLever.PositionIsInWarningRange())
                {
                    numOfLeversInWarnRange++;
                }
            }

            return numOfLeversInWarnRange;
        }

        protected override void ChangeInteractive(bool newState)
        {
            foreach (var element in interactiveElementsParents)
            {
                element.SetActive(false);
            }
            
            if (newState)
            {
                for (int i = 0; i < Config.leverQuantity; i++)
                {
                    interactiveElementsParents[i].SetActive(true);
                }
            }
        }

        protected override int GetNumberOfFails()
        {
            int numOfLeversInFailRange = 0;

            foreach (var rodLever in levers)
            {
                if (rodLever.PositionIsInDangerRange())
                {
                    numOfLeversInFailRange++;
                }
            }

            return numOfLeversInFailRange;
        }

        protected override IEnumerator ResetLogicCoroutine()
        {
            foreach (var rodLever in levers)
            {
                StartCoroutine(rodLever.ResetLever());
            }

            yield return new WaitForSeconds(Config.resetWaitingTime);
        }
    }

    [Serializable]
    public class RodPositioningConfig : GeneralConfigBase
    {
        public float maxRangeValue = 1f;
        public float minRangeValue;
        [Range(0, 1)] public float minSafeRangeSize = 0.15f;
        [Range(0, 1)] public float maxSafeRangeSize = 0.2f;
        [Range(0, 1)] public float minDangerRangeSize = 0.1f;
        [Range(0, 1)] public float warningRangeMinForSingle = 0.2f;
        [Range(0, 1)] public float warningRangeMaxForSingle = 0.4f;
        [Range(0, 1)] public float warningRangeMinForBoth = 0.1f;
        [Range(0, 1)] public float warningRangeMaxForBoth = 0.2f;
        [Range(0, 1)] public float maxStepLength = 0.001f;
        public int leverQuantity;
    }
}