using UnityEngine.Events;

namespace Resource_Nodes
{
    public interface IDestructible
    {
        public float MaxHp { get; }
        public float CurrentHp { get; set; }
        // IInstrument CurrentInstrument { get; set; } 

        public void OnLaserInteraction(float damage)
        {
            TakeDamage(damage);
        }
        
        public void OnDrillInteraction(float damage)
        {
            TakeDamage(damage);
        }
        
        public void OnCutterInteraction(float damage)
        {
            TakeDamage(damage);
        }
        public void OnExplosivesInteraction(float damage)
        {
            TakeDamage(damage);
        }

        private void TakeDamage(float damage)
        {
            CurrentHp -= damage;
            
            if (CurrentHp <= 0)
            {
                InitiateDestroy();
            }
        }

        public UnityEvent OnDestroyed { get; }

        public void InitiateDestroy()
        {
            OnDestroyed.Invoke();
        }
    }
}
