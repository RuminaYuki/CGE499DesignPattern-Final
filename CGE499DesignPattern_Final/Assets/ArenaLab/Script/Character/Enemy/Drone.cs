using UnityEngine;

public class Drone : MonoBehaviour, IDamageable,IVisitable
{
    private HealthSystem hs;

    [Header("Health")]
    [SerializeField] private float maxHealth = 3f;
    private bool isDead;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;
    private Movement mv;

    [Header("Aim / Shoot")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRadius = 1f;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletDamage = 1f;

    private float shootTimer;

    public float Damage
    {
        get => bulletDamage;
        set => bulletDamage = value;
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mv = new Movement(rb, moveSpeed);
        hs = new HealthSystem(maxHealth);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void OnEnable()
    {
        if (hs != null)
            hs.OnDied += Death;
    }

    private void OnDisable()
    {
        if (hs != null)
            hs.OnDied -= Death;
    }

    private void Start()
    {
        isDead = false;
        shootTimer = shootCooldown;
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        Move();
        AimFirePoint();
        Shoot();
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    private void Move()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        mv.Move(dir);
    }

    private void AimFirePoint()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;

        firePoint.localPosition = dir * fireRadius;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Shoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer > 0f)
            return;

        shootTimer = shootCooldown;

        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.Initialize(dir, bulletSpeed, bulletDamage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        hs.TakeDamage(damage);
    }

    private void Death()
    {
        isDead = true;
        mv.Stop();
        Destroy(gameObject);
    }
}