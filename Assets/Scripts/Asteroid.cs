using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int asteroidPointsCount = 0;

    private void Start()
    {
        // Count the initial number of asteroid points
        asteroidPointsCount = 3;
        Debug.Log(asteroidPointsCount);
    }

    public void OnAsteroidPointDestroyed()
    {
        // Decrement the count of remaining asteroid points
        Debug.Log(asteroidPointsCount);
        asteroidPointsCount--;

        // Check if all points are destroyed
        if (asteroidPointsCount <= 0)
        {
            // Shatter the entire asteroid into random pieces
            ShatterAsteroid();
        }
    }

    void ShatterAsteroid()
    {
        // Add logic to shatter the entire asteroid into random pieces
        // This can involve creating new GameObjects representing shattered pieces
        // and positioning them in a visually appealing manner.
        Debug.Log("Asteroid shattered!");

        // Destroy the entire asteroid GameObject
        Destroy(gameObject);
    }
}
