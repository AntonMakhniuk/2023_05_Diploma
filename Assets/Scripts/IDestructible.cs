using Assets.Scripts.Instruments;
using UnityEngine;

public interface IDestructible
{
    double MaxHP { get; set; }
    double CurrentHP { get;  set; }
   // IInstrument CurrentInstrument { get; set; } 

    void OnLaserInteraction(double damage);
    void OnDrillInteraction(double damage);
    void OnCutterInteraction(double damage);
    void OnExplosivesInteraction(double damage);
    void TakeDamage(double damage);
    void InitiateDestroy();
}
