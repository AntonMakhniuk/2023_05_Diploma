using System;
using UnityEngine;

namespace Production.Challenges
{
    public abstract class GeneralBase : MonoBehaviour
    {
        private void Update()
        {
            UpdateChallenge();
        }

        protected abstract void UpdateChallenge(); 
     
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler GeneralFailed;
        
        private void Fail()
        {
            GeneralFailed?.Invoke();
            Reset();
        }

        protected abstract void Reset();
    }
}