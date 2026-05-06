using UnityEngine;

public abstract class BounceStrategyDecorator : IBounceStrategy
{
    protected readonly IBounceStrategy inner;

    protected BounceStrategyDecorator(IBounceStrategy inner)
    {
        this.inner = inner;
    }

    public virtual Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal)
    {
        return inner.CalculateSurfaceBounce(incomingDirection, normal);
    }

    public virtual Vector2 CalculatePaddleBounce(
        float ballX,
        float paddleX,
        float paddleWidth,
        float paddleVelocityX,
        float paddleInfluence
    )
    {
        return inner.CalculatePaddleBounce(
            ballX,
            paddleX,
            paddleWidth,
            paddleVelocityX,
            paddleInfluence
        );
    }

    public virtual float ModifySpeed(float currentSpeed)
    {
        return inner.ModifySpeed(currentSpeed);
    }

    public virtual float GetBaseSpeed()
    {
        return inner.GetBaseSpeed();
    }
}
