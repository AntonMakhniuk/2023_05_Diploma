using UnityEngine;

namespace Production.Challenges
{
    public abstract class GeneralBase : MonoBehaviour
    {
        private void FixedUpdate()
        {
            UpdateChallenge();
        }

        protected abstract void UpdateChallenge(); 
     
        public delegate void GeneralFailHandler();
        public event GeneralFailHandler GeneralFailed;
        
        protected void Fail()
        {
            GeneralFailed?.Invoke();
            Reset();
        }

        protected abstract void Reset();
    }
}