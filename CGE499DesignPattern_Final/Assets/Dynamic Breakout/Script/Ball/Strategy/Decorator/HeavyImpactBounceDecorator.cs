using UnityEngine;

public class HeavyImpactBounceDecorator : BounceStrategyDecorator
{
    private readonly float horizontalWeight;

    public HeavyImpactBounceDecorator(IBounceStrategy inner, float horizontalWeight)
        : base(inner)
    {
        this.horizontalWeight = Mathf.Clamp(horizontalWeight, 0.1f, 1f);
    }

    public override Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal)
    {
        Vector2 reflected = inner.CalculateSurfaceBounce(incomingDirection, normal);
        reflected.x *= horizontalWeight;
        return reflected.normalized;
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
        baseDirection.x *= horizontalWeight;
        return baseDirection.normalized;
    }
}
