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

    private float timer;
    private BounceRuleType currentRule;

    private void Start()
    {
        timer = 0f;
        currentRule = BounceRuleType.Normal;
        
        ball.SetStrategy<NormalBounceStrategy>();
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

        switch (nextRule)
        {
            case BounceRuleType.Normal:
                ball.SetStrategy<NormalBounceStrategy>();
                break;

            case BounceRuleType.SpeedUp:
                ball.SetStrategy<SpeedUpBounceStrategy>();
                break;

            case BounceRuleType.Heavy:
                ball.SetStrategy<HeavyBounceStrategy>();
                break;
        }

        Debug.Log($"Rule Changed To: {nextRule}");
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