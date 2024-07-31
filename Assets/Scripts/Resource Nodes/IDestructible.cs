using UnityEngine;

public interface IDestructible
{
    float MaxHP { get; set; }
    float CurrentHP { get;  set; }
   // IInstrument CurrentInstrument { get; set; } 

    void OnLaserInteraction(float damage);
    void OnDrillInteraction(float damage);
    void OnCutterInteraction(float damage);
    void OnExplosivesInteraction(float damage);
    void TakeDamage(float damage);
    void InitiateDestroy();
}
