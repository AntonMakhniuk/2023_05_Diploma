using UnityEngine;

namespace Production.Challenges
{
    public abstract class ResourceBase<TConfig> : MonoBehaviour where TConfig : ConfigBase
    {
        public ResourceType type;
        
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

    public enum ResourceType
    {
        Ore, Gas
    }
}