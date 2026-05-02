using UnityEngine;

public class NormalBounceStrategy : MonoBehaviour, IBounceStrategy
{
    [SerializeField] private float baseSpeed = 5f;

    public Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal)
    {
        return Vector2.Reflect(incomingDirection, normal).normalized;
    }

    public Vector2 CalculatePaddleBounce(
        float ballX,
        float paddleX,
        float paddleWidth,
        float paddleVelocityX,
        float paddleInfluence
    )
    {
        float offset = ballX - paddleX;
        float halfWidth = paddleWidth * 0.5f;
        float normalizedOffset = Mathf.Clamp(offset / halfWidth, -1f, 1f);

        Vector2 newDirection = new Vector2(
            normalizedOffset + paddleVelocityX * paddleInfluence,
            1f
        ).normalized;

        return newDirection;
    }

    public float ModifySpeed(float currentSpeed)
    {
        return currentSpeed;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }
}