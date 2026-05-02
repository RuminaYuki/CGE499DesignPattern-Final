using UnityEngine;

public interface IBounceStrategy
{
    Vector2 CalculateSurfaceBounce(Vector2 incomingDirection, Vector2 normal);

    Vector2 CalculatePaddleBounce(
        float ballX,
        float paddleX,
        float paddleWidth,
        float paddleVelocityX,
        float paddleInfluence
    );

    float ModifySpeed(float currentSpeed);

    float GetBaseSpeed();
}