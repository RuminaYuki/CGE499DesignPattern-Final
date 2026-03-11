using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour,IEnemy, IDamageable, IVisitable
{
    private HealthSystem hs;
    public HealthSystem Hs => hs;

    [Header("Health")]
    [SerializeField] private float maxHealth = 3f;
    private bool isDead;

    [Header("Detect Player")]
    [SerializeField] private float detectRadius = 6f;
    private Transform player;

    [Header("Shoot")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRadius = 1f;
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
        set
        {
            float previous = maxHealth;
            maxHealth = value;

            if (hs != null && value > previous)
            {
                hs.IncreaseMaxHealth(value - previous);
            }
        }
    }
    public float CurrentHealth => hs != null ? hs.CurrentHP : maxHealth;
    public float MovementSpeed => 0f;
    public IReadOnlyList<string> Upgrades => upgrades;

    private readonly List<string> upgrades = new();

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
        
        TryFindPlayer();
        if (player == null) return;

        DetectPlayer();
    }

    void TryFindPlayer()
{
    if (player != null) return;

    GameObject p = GameObject.FindGameObjectWithTag("Player");
    if (p != null)
        player = p.transform;
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

        firePoint.localPosition = dir * fireRadius;

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
        CharacterAudio.instance.PlayShotSound();
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
        CharacterAudio.instance.PlayDieSound();
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

    public void RecordUpgrade(string upgradeText)
    {
        if (string.IsNullOrWhiteSpace(upgradeText)) return;
        upgrades.Add(upgradeText);
    }
}
