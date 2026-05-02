using UnityEngine;

public class DefaultBounceCalculator
{
    public Vector2 Calculate(Vector2 currentVelocity, Vector2 normal)
    {
        return Vector2.Reflect(currentVelocity, normal).normalized;
    }
}
