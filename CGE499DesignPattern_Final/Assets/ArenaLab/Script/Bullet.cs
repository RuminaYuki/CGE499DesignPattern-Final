using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private Vector2 moveDirection;
    private float moveSpeed;
    private float damage;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, float damageAmount)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        damage = damageAmount;

        // หมุนกระสุนให้หันไปทางที่ยิง
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // ใช้ Rigidbody ยิงออกไป
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}