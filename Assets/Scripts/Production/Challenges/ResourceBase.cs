using UnityEngine;

namespace Production.Challenges
{
    public abstract class ResourceBase : MonoBehaviour
    {
        private void FixedUpdate()
        {
            UpdateChallenge();
        }

        protected abstract void UpdateChallenge(); 
        
        protected void Fail()
        {
            Reset();
        }

        protected abstract void Reset();
    }
}