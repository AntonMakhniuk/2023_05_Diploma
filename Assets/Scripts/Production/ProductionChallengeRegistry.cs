using System;
using System.Linq;
using Production.Challenges;
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
                if (!challengePrefab.GetComponents<Component>()
                        .Any(comp => comp.GetType().IsGenericType &&
                                     comp.GetType().GetGenericTypeDefinition() == typeof(GeneralBase<>) &&
                                     typeof(ConfigBase).IsAssignableFrom(comp.GetType().GetGenericArguments()[0])))
                {
                    throw new ArgumentException("The list of general challenges cannot contain objects" +
                                                "that do not have a component inheriting from GeneralBase attached.");
                }
            }
            
            foreach (GameObject challengePrefab in resourceChallengePrefabs)
            {
                if (!challengePrefab.GetComponents<Component>()
                        .Any(comp => comp.GetType().IsGenericType &&
                                     comp.GetType().GetGenericTypeDefinition() == typeof(ResourceBase<>) &&
                                     typeof(ConfigBase).IsAssignableFrom(comp.GetType().GetGenericArguments()[0])))
                {
                    throw new ArgumentException("The list of resource challenges cannot contain objects" +
                                                "that do not have a component inheriting from ResourceBase attached.");
                }
            }
        }
    }
}