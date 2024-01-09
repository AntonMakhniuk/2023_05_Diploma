using System;
using Production.Challenges;
using UnityEngine;

namespace Production
{
    public class ProductionStateTracker : MonoBehaviour
    {
        private int _criticalFailCount;
        [SerializeField] private GeneralBase[] _generalChallenges;
        
        private void Start()
        {
            foreach (GeneralBase general in _generalChallenges)
            {
                general.GeneralFailed += AddCriticalFail;
            }
        }

        public void OnDestroy()
        {
            foreach (GeneralBase general in _generalChallenges)
            {
                general.GeneralFailed -= AddCriticalFail;
            }
        }

        private void AddCriticalFail()
        {
            if (_criticalFailCount < 3)
            {
                _criticalFailCount++;
            }
            else
            {
                FailProduction();
            }
        }

        private void FailProduction()
        {
            throw new NotImplementedException();
        }
    }
}