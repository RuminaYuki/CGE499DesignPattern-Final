using System.Collections;
using UnityEngine;

public class BricksRenderer : MonoBehaviour
{
    public enum BrickColorType
    {
        Cray,Green,Orange,Purple,Red,Yellow
    }
    [System.Serializable]
    public class BrickSpriteSet
    {
        public BrickColorType type;
        public Sprite normal;
        public Sprite damaged;
        public Sprite critical;
    }

    [Header("Current Type")]
    [SerializeField] private BrickColorType currentType;

    [Header("Sprite Library")]
    [SerializeField] private BrickSpriteSet[] spriteSets;

    private Brick brick;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        brick = GetComponentInParent<Brick>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnDisable()
    {
        brick.BrickHp.OnHealthChanged -= (float _) => UpdateSprite();
        brick.BrickHp.OnHealthChanged -= (float _) => OnHit();
    }
    void Start()
    {
        brick.BrickHp.OnHealthChanged += (float _) => UpdateSprite();
        brick.BrickHp.OnHealthChanged += (float _) => OnHit();
        UpdateSprite();
    }
    private void UpdateSprite()
    {
        BrickSpriteSet set = GetSpriteSetByType(currentType);
        if (set == null) return;

        float currentHp = brick.BrickHp.CurrentHP;
        float hpPercent = brick.BrickHp.CurrentHP / brick.BrickHp.MaxHP;

        if (hpPercent > 0.66f)
        {
            spriteRenderer.sprite = set.normal;
        }
        else if (hpPercent > 0.33f)
        {
            spriteRenderer.sprite = set.damaged;
        }
        else if (currentHp > 0)
        {
           spriteRenderer.sprite = set.critical;
        }
    }
    private void OnHit()
    {
        StartCoroutine(ChangeAlpha());
    }
    IEnumerator ChangeAlpha()
    {
        Color original = spriteRenderer.color;

        Color c = original;
        c.a = 0.5f;
        spriteRenderer.color = c;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.color = original;
    }
    private BrickSpriteSet GetSpriteSetByType(BrickColorType type)
    {
        for (int i = 0; i < spriteSets.Length; i++)
        {
            if (spriteSets[i].type == type)
                return spriteSets[i];
        }

        return null;
    }
}
