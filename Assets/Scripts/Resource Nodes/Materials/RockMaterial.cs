using UnityEngine;
using UnityEngine.Events;

namespace Resource_Nodes.Materials
{
    public class RockMaterial : MonoBehaviour, IDestructible
    {
        [SerializeField] private float maxHp;
        public float MaxHp => maxHp;

        [SerializeField] private float currentHp;
        public float CurrentHp
        {
            get => currentHp;
            set => currentHp = value;
        }
        
        [SerializeField] private UnityEvent onDestroyed;
        public UnityEvent OnDestroyed => onDestroyed;

        private void Start()
        {
            CurrentHp = MaxHp;
        }
    }
}
