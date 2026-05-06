using UnityEngine;

public class RandomAngleBounceDecorator : BounceStrategyDecorator
{
    private readonly float maxAngleOffset;

    public RandomAngleBounceDecorator(IBounceStrategy inner, float maxAngleOffset)
        : base(inner)
    {
        this.maxAngleOffset = Mathf.Abs(maxAngleOffset);
    }

    public override Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal)
    {
        Vector2 baseDirection = inner.CalculateSurfaceBounce(incomingDirection, normal);
        return ApplyAngleOffset(baseDirection);
    }

    public override Vector2 CalculatePaddleBounce(
        float ballX,
        float paddleX,
        float paddleWidth,
        float paddleVelocityX,
        float paddleInfluence
    )
    {
        Vector2 baseDirection = inner.CalculatePaddleBounce(
            ballX,
            paddleX,
            paddleWidth,
            paddleVelocityX,
            paddleInfluence
        );

        return ApplyAngleOffset(baseDirection);
    }

    private Vector2 ApplyAngleOffset(Vector2 direction)
    {
        float angleOffset = Random.Range(-maxAngleOffset, maxAngleOffset);
        return Quaternion.Euler(0f, 0f, angleOffset) * direction.normalized;
    }
}
