using UnityEngine;

public class HeavyBounceStrategy : MonoBehaviour, IBounceStrategy
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float horizontalWeight = 0.5f;

    [SerializeField] private float upwardSpeedMultiplier = 1f;
    [SerializeField] private float downwardSpeedMultiplier = 1.35f;

    private float lastDirectionY = 1f;

    public Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal)
    {
        Vector2 reflected = Vector2.Reflect(incomingDirection, normal);
        reflected.x *= horizontalWeight;

        Vector2 finalDirection = reflected.normalized;
        lastDirectionY = finalDirection.y;

        return finalDirection;
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
            (normalizedOffset + paddleVelocityX * paddleInfluence) * horizontalWeight,
            1f
        ).normalized;

        lastDirectionY = newDirection.y;
        return newDirection;
    }

    public float ModifySpeed(float currentSpeed)
    {
        if (lastDirectionY < 0f)
            return baseSpeed * downwardSpeedMultiplier;

        return baseSpeed * upwardSpeedMultiplier;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }
}