using System.Collections.Generic;
using Production.Challenges;
using UnityEngine;

namespace Production
{
    public class ProductionChallengeRegistry : MonoBehaviour
    {
        private static ProductionChallengeRegistry _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GeneralBase[] generalChallenges;
        public ResourceBase[] resourceChallenges;
    }
}