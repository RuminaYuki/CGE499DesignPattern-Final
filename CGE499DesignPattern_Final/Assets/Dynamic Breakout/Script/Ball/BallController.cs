using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    
    private float speed;
    [SerializeField] private int damage = 1;
    [SerializeField] private float paddleInfluence = 0.15f;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private IBounceStrategy bounceStrategy;

    public Action OnDie;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStrategy<NormalBounceStrategy>();
    }

    void Start()
    {
        StartBallMove();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            lastVelocity = rb.linearVelocity;
        }
    }

    void Update()
    {
        // ทดสอบเปลี่ยน strategy runtime
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetStrategy<NormalBounceStrategy>();
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetStrategy<SpeedUpBounceStrategy>();
            Debug.Log("Bounce Strategy: SpeedUp");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetStrategy<HeavyBounceStrategy>();
            Debug.Log("Bounce Strategy: Heavy");
        }
        */
    }
    public void SetStrategy<T>() where T : MonoBehaviour, IBounceStrategy
    {
        T newStrategy = GetComponent<T>();
        if (newStrategy == null)
        {
            Debug.LogWarning($"Strategy {typeof(T).Name} not found on {gameObject.name}");
            return;
        }

        bounceStrategy = newStrategy;
        speed = bounceStrategy.GetBaseSpeed();

        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    void StartBallMove()
    {
        Vector2 direction = new Vector2(0, 1);
        rb.linearVelocity = direction.normalized * speed;
    }

    private void BounceFromPaddle(Collision2D collision)
    {
        PaddleController paddle = collision.gameObject.GetComponent<PaddleController>();
        if (paddle == null) return;

        float paddleX = collision.transform.position.x;
        float ballX = transform.position.x;
        float paddleWidth = collision.collider.bounds.size.x;
        float paddleVelocityX = paddle.CurrentVelocity.x;

        Vector2 newDirection = bounceStrategy.CalculatePaddleBounce(
            ballX,
            paddleX,
            paddleWidth,
            paddleVelocityX,
            paddleInfluence
        );

        speed = bounceStrategy.ModifySpeed(speed);
        rb.linearVelocity = newDirection * speed;
    }

    private void BounceFromSurface(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 incomingDirection = lastVelocity.normalized;

        Vector2 newDirection = bounceStrategy.CalculateSurfaceBounce(
            incomingDirection,
            normal
        );

        speed = bounceStrategy.ModifySpeed(speed);
        rb.linearVelocity = newDirection * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bricks") &&
            collision.gameObject.TryGetComponent(out IDamageable obj))
        {
            obj.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Paddle"))
        {
            BounceFromPaddle(collision);
            return;
        }

        if (collision.gameObject.CompareTag("EndPath"))
        {
            OnDie?.Invoke();
            Destroy(gameObject);
            return;
        }

        BounceFromSurface(collision);
    }
}