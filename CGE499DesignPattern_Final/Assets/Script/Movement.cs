using UnityEngine;

public class Movement
{
    private Rigidbody2D _rb;
    public float Speed { get; set; }

    public Movement(Rigidbody2D rb, float speed)
    {
        _rb = rb;
        Speed = speed;
    }

    public void Move(Vector2 direction)
    {
        _rb.linearVelocity = direction * Speed;
    }

    public void Stop()
    {
        _rb.linearVelocity = Vector2.zero;
    }
}