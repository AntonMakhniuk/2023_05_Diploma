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
    }

    public void SetUp(Asteroid asteroid)
    {
        parentAsteroid = asteroid;
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
            InitiateDestroy();
        }
    }

    public void InitiateDestroy()
    {
        if (parentAsteroid != null)
        {
            parentAsteroid.OnAsteroidPointDestroyed();
        }
        Destroy(gameObject);
    }
}
