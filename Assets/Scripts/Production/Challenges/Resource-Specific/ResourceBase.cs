using UnityEngine;

namespace Production.Challenges.ResourceSpecific
{
    public abstract class ResourceBase<TConfig> : MonoBehaviour, IResourceChallenge where TConfig : ConfigBase
    {
        public ResourceType resourceType;
        
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

        public ResourceType GetResourceType()
        {
            return resourceType;
        }
    }

    public interface IResourceChallenge
    {
        public ResourceType GetResourceType();
    }

    public enum ResourceType
    {
        Ore, Gas
    }
}