using UnityEngine;

namespace Production.Challenges
{
    public abstract class ResourceBase<TConfig> : MonoBehaviour, IResourceChallenge where TConfig : ConfigBase
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

    public interface IResourceChallenge
    {
        
    }

    public enum ResourceType
    {
        Ore, Gas
    }
}