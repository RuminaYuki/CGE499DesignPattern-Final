using UnityEngine;

public class PaddleBounceCalculator
{
    public Vector2 Calculate(
        float ballX,
        float paddleX,
        float paddleWidth,
        float paddleVelocityX,
        float paddleInfluence)
    {
        float offset = ballX - paddleX;
        float halfWidth = paddleWidth * 0.5f;

        float normalizedOffset = offset / halfWidth;
        normalizedOffset = Mathf.Clamp(normalizedOffset, -1f, 1f);

        Vector2 newDirection = new Vector2(
            normalizedOffset + paddleVelocityX * paddleInfluence,
            1f
        ).normalized;

        return newDirection;
    }
}
