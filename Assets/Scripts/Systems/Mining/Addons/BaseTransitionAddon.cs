using UnityEngine;

namespace Systems.Mining.Addons
{
    public abstract class BaseTransitionAddon : MonoBehaviour
    {
        [SerializeField] private BaseTransitionAddon nextTransition;
        
        public virtual void ApplyEffect()
        {
            if (nextTransition != null)
            {
                nextTransition.ApplyEffect();
            }
        }
    }
}