using System;
using Production.Challenges;
using Production.Challenges.General;
using UnityEngine;

namespace Production
{
    public class ProductionChallengeRegistry : MonoBehaviour
    {
        private static ProductionChallengeRegistry _instance;
        
        public GameObject[] generalChallengePrefabs;
        public GameObject[] resourceChallengePrefabs;
        
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

            VerifyChallenges();
        }

        private void VerifyChallenges()
        {
            foreach (GameObject challengePrefab in generalChallengePrefabs)
            {
                if (challengePrefab.GetComponent<IGeneralChallenge>() == null)
                {
                    throw new ArgumentException("The list of general challenges cannot contain objects" +
                                                "that do not have a component implementing IGeneralChallenge attached.");
                }
            }
            
            foreach (GameObject challengePrefab in resourceChallengePrefabs)
            {
                if (challengePrefab.GetComponent<IResourceChallenge>() == null)
                {
                    throw new ArgumentException("The list of resource challenges cannot contain objects" +
                                                "that do not have a component implementing IResourceChallenge attached.");
                }
            }
        }
    }
}