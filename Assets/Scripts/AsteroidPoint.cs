using Assets.Scripts.Instruments;
using UnityEngine;

public class AsteroidPoint : MonoBehaviour, IDestructible
{
    public double MaxHP { get; set; } = 10;
    public double CurrentHP { get; set; }
    //public IInstrument CurrentInstrument { get; set; }

    private Asteroid parentAsteroid;

    private void Start()
    {
        CurrentHP = MaxHP;
        parentAsteroid = GetComponentInParent<Asteroid>();
    }

    public void OnLaserInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void OnDrillInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void OnCutterInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void OnExplosivesInteraction(double damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(double damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (parentAsteroid != null)
        {
            parentAsteroid.OnAsteroidPointDestroyed();
        }
        Destroy(gameObject);
    }
}
