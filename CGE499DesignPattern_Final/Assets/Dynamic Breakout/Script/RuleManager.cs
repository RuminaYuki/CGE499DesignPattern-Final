using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public enum BounceRuleType
    {
        Normal,
        SpeedUp,
        Heavy
    }

    [Header("References")]
    [SerializeField] private BallController ball;

    [Header("Rule Settings")]
    [SerializeField] private float ruleChangeInterval = 10f;
    [SerializeField] private bool avoidRepeat = true;

    [Header("Decorator Settings")]
    [SerializeField] private bool useSpeedDecorator = true;
    [SerializeField] private float speedBonus = 0.25f;
    [SerializeField] private float maxDecoratorSpeed = 20f;

    [SerializeField] private bool useRandomAngleDecorator = false;
    [SerializeField] private float randomAngleOffset = 6f;

    [SerializeField] private bool useHeavyImpactDecorator = false;
    [SerializeField] private float heavyHorizontalWeight = 0.7f;

    private float timer;
    private BounceRuleType currentRule;

    private void Start()
    {
        timer = 0f;
        currentRule = BounceRuleType.Normal;

        ball.SetStrategy(BuildStrategy(currentRule));
        Debug.Log("Rule Changed To: Normal");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= ruleChangeInterval)
        {
            timer = 0f;
            ApplyRandomRule(avoidRepeat);
        }
    }

    private void ApplyRandomRule(bool forceDifferent)
    {
        BounceRuleType nextRule = GetRandomRule(forceDifferent);
        currentRule = nextRule;
        ball.SetStrategy(BuildStrategy(nextRule));

        Debug.Log($"Rule Changed To: {nextRule}");
    }

    private IBounceStrategy BuildStrategy(BounceRuleType ruleType)
    {
        IBounceStrategy strategy = GetBaseStrategy(ruleType);

        if (strategy == null)
        {
            Debug.LogWarning($"Base strategy for {ruleType} is missing. Fallback to Normal.");
            strategy = ball.GetStrategyComponent<NormalBounceStrategy>();
        }

        if (strategy == null)
        {
            Debug.LogWarning("NormalBounceStrategy component is missing on Ball.");
            return null;
        }

        if (useSpeedDecorator)
        {
            strategy = new SpeedBoostBounceDecorator(strategy, speedBonus, maxDecoratorSpeed);
        }

        if (useRandomAngleDecorator)
        {
            strategy = new RandomAngleBounceDecorator(strategy, randomAngleOffset);
        }

        if (useHeavyImpactDecorator)
        {
            strategy = new HeavyImpactBounceDecorator(strategy, heavyHorizontalWeight);
        }

        return strategy;
    }

    private IBounceStrategy GetBaseStrategy(BounceRuleType ruleType)
    {
        switch (ruleType)
        {
            case BounceRuleType.Normal:
                return ball.GetStrategyComponent<NormalBounceStrategy>();
            case BounceRuleType.SpeedUp:
                return ball.GetStrategyComponent<SpeedUpBounceStrategy>();
            case BounceRuleType.Heavy:
                return ball.GetStrategyComponent<HeavyBounceStrategy>();
            default:
                return ball.GetStrategyComponent<NormalBounceStrategy>();
        }
    }

    private BounceRuleType GetRandomRule(bool forceDifferent)
    {
        BounceRuleType newRule;

        do
        {
            newRule = (BounceRuleType)Random.Range(0, 3);
        }
        while (forceDifferent && newRule == currentRule);

        return newRule;
    }
}
