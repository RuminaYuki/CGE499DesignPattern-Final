using UnityEngine;
using System.Collections.Generic;

public class Drone : MonoBehaviour,IEnemy , IDamageable, IVisitable
{
    private HealthSystem hs;
    public HealthSystem Hs => hs;

    [Header("Health")]
    [SerializeField] private float maxHealth = 3f;
    private bool isDead;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 2f;
    private Transform player;
    private Rigidbody2D rb;
    private Movement mv;

    [Header("Aim / Shoot")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRadius = 1f;
    [SerializeField] private float minShootCooldown = 0.8f;
    [SerializeField] private float maxShootCooldown = 1.2f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletDamage = 1f;

    private float shootTimer;

    public float Damage
    {
        get => bulletDamage;
        set => bulletDamage = value;
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
    public Movement Mv => mv;
    public float CurrentHealth => hs != null ? hs.CurrentHP : maxHealth;
    public float MovementSpeed => mv != null ? mv.Speed : moveSpeed;
    public IReadOnlyList<string> Upgrades => upgrades;

    private readonly List<string> upgrades = new();

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
        shootTimer = Random.Range(minShootCooldown, maxShootCooldown);
    }

    private void Update()
    {
        if (isDead) return;

        TryFindPlayer();
        if (player == null) return;

        Move();
        AimFirePoint();
        Shoot();
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

    private void Move()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        if (distance <= stopDistance)
        {
            mv.Stop();
            return;
        }

        Vector2 dir = toPlayer.normalized;
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

        shootTimer = Random.Range(minShootCooldown, maxShootCooldown);

        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.Initialize(dir, bulletSpeed, bulletDamage);
        CharacterAudio.instance.PlayShotSound();
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
        CharacterAudio.instance.PlayDieSound();
        Destroy(gameObject);
    }

    public void RecordUpgrade(string upgradeText)
    {
        if (string.IsNullOrWhiteSpace(upgradeText)) return;
        upgrades.Add(upgradeText);
    }
}
