using System;
using System.Collections.Generic;
using System.Linq;
using Production.Challenges.General;
using Production.Challenges.ResourceSpecific;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Production.Systems
{
    public class ProductionChallengeRegistry : MonoBehaviour
    {
        public GameObject[] generalChallengePrefabs;
        public GameObject[] resourceChallengePrefabs;
        
        private void Awake()
        {
            ValidateChallenges();
        }

        private void ValidateChallenges()
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

        public GameObject[] GetNumberOfRandomGeneralChallenges(int numberOfChallenges)
        {
            var availableChallenges = new List<GameObject>(generalChallengePrefabs);

            if (numberOfChallenges > availableChallenges.Count)
            {
                Debug.LogError("Tried to access more challenges than are available in the registry. " +
                               "Will return maximum possible amount.");
            }
                
            List<GameObject> chosenChallenges = new(numberOfChallenges);

            for (int i = 0; i < numberOfChallenges && availableChallenges.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, availableChallenges.Count);
                
                chosenChallenges.Add(availableChallenges[randomIndex]);
                availableChallenges.RemoveAt(randomIndex);
            }

            return chosenChallenges.ToArray();
        }
        
        public GameObject[] GetPermittedResourceChallenges(ResourcePlaceholder[] resources)
        {
            return resourceChallengePrefabs
                .Where(prefab => 
                    resources.Any(res =>
                        prefab.GetComponent<IResourceChallenge>().GetResourceType() == res.Type))
                .ToArray();
        }
    }
}