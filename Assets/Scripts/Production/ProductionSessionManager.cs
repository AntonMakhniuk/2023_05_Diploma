using System;
using Production.Challenges;
using UnityEngine;

namespace Production
{
    // meant to be created every time a new production is started, and destroyed afterwards
    public class ProductionSessionManager : MonoBehaviour
    {
        // TODO: change this to prefab
        [SerializeField] private GeneralBase[] generalChallenges;
        [SerializeField] private int maxCriticalFails = 3;
        
        public Difficulty difficulty;
        
        private int _criticalFailCount;
        
        private void Start()
        {
            foreach (GeneralBase general in generalChallenges)
            {
                general.GeneralFailed += AddCriticalFail;
            }
        }

        public void OnDestroy()
        {
            foreach (GeneralBase general in generalChallenges)
            {
                general.GeneralFailed -= AddCriticalFail;
            }
        }

        private void AddCriticalFail()
        {
            if (_criticalFailCount < maxCriticalFails)
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