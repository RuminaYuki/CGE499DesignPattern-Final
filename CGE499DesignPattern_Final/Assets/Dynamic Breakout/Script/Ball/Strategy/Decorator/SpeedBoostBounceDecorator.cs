using UnityEngine;

public class SpeedBoostBounceDecorator : BounceStrategyDecorator
{
    private readonly float bonusSpeed;
    private readonly float maxSpeed;

    public SpeedBoostBounceDecorator(IBounceStrategy inner, float bonusSpeed, float maxSpeed)
        : base(inner)
    {
        this.bonusSpeed = bonusSpeed;
        this.maxSpeed = maxSpeed;
    }

    public override float ModifySpeed(float currentSpeed)
    {
        float speedFromInner = inner.ModifySpeed(currentSpeed);
        return Mathf.Min(speedFromInner + bonusSpeed, maxSpeed);
    }
}
