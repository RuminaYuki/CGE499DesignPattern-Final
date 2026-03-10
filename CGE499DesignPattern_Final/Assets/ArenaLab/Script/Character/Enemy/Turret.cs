using UnityEngine;

public class Turret : MonoBehaviour, IDamageable, IVisitable
{
    private HealthSystem hs;

    [Header("Health")]
    [SerializeField] private float maxHealth = 3f;
    private bool isDead;

    [Header("Detect Player")]
    [SerializeField] private float detectRadius = 6f;
    private Transform player;

    [Header("Shoot")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float bulletDamage = 1f;

    private float shootTimer;

    // Visitor use
    public float Damage
    {
        get => bulletDamage;
        set => bulletDamage = value;
    }
    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    void Awake()
    {
        hs = new HealthSystem(maxHealth);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void OnEnable()
    {
        hs.OnDied += Death;
    }

    void OnDisable()
    {
        hs.OnDied -= Death;
    }

    void Start()
    {
        isDead = false;
        shootTimer = fireRate;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        DetectPlayer();
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    // ======================
    // Detect Player
    // ======================
    void DetectPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRadius)
        {
            Aim();
            Shoot();
        }
    }

    // ======================
    // Aim at Player
    // ======================
    void Aim()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // ======================
    // Shoot
    // ======================
    void Shoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer > 0) return;

        shootTimer = fireRate;

        Vector2 dir = (player.position - firePoint.position).normalized;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.Initialize(dir, bulletSpeed, bulletDamage);
    }

    // ======================
    // Damage
    // ======================
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hs.TakeDamage(damage);
    }

    void Death()
    {
        isDead = true;
        Destroy(gameObject);
    }

    // ======================
    // Debug Radius
    // ======================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}