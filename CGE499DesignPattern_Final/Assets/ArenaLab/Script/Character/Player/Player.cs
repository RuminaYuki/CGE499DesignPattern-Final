using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoBehaviour,IDamageable,IVisitable
{
    
    HealthSystem hs;
    public float maxHealth = 3;

    //movement
    Rigidbody2D rb;
    Movement mv;
    public float walkspeed = 3;

    //aim
    [Header("Aim")]
    Camera mainCam;
    Vector2 dir;
    public Transform aim;
    public float aimRadius = 1f;

    [Header("Shoot")]
    public bool canFire;
    private float timer;
    public float fireCooldown;
    public Bullet bulletPrefab;
    public float damage = 1;
    public float bulletSpeed = 10f;

    private bool isDeath;

    // Visitor use
    public float Damage
    {
        get => damage;
        set => damage = value;
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
    public HealthSystem Hs => hs;
    public float CurrentHealth => hs != null ? hs.CurrentHP : maxHealth;
    public float MovementSpeed => mv != null ? mv.Speed : walkspeed;
    public IReadOnlyList<string> Upgrades => upgrades;

    private readonly List<string> upgrades = new();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hs = new (maxHealth);
        mv = new (rb,walkspeed);

        mainCam = GetComponentInChildren<Camera>();
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
        isDeath = false;
    }

    void Update()
    {
        if(isDeath) return;
        Walk();
        Aim();
        Shoot();
    }

    public void Accept(IVisitor visior)
    {
        visior.Visit(this);
    }

    void Walk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(horizontal,vertical).normalized;

        mv.Move(dir);
        
    }

    void Aim()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCam.transform.position.z;

        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        dir = (mouseWorldPos - transform.position).normalized;

        // ทำให้ปืนไปอยู่รอบ Player
        aim.position = transform.position + (Vector3)(dir * aimRadius);

        // ทำให้ปืนหันไปทางเมาส์
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        aim.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }

    void Shoot()
    {
        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > fireCooldown)
            {
                canFire = true;
                timer = 0;
            }
        }
        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            
            Bullet bullet = Instantiate(bulletPrefab,aim.position,Quaternion.identity);
            bullet.Initialize(dir,bulletSpeed,damage);
            CharacterAudio.instance.PlayShotSound();
        }
    }

    // IDamageable
    public void TakeDamage(float damage)
    {
        hs.TakeDamage(damage);
    }

    void Death()
    {
        mv.Stop();
        Camera.main.transform.SetParent(null);
        CharacterAudio.instance.PlayDieSound();
        gameObject.SetActive(false);
        isDeath = true;
    }

    public void RecordUpgrade(string upgradeText)
    {
        if (string.IsNullOrWhiteSpace(upgradeText)) return;
        upgrades.Add(upgradeText);
    }
}
