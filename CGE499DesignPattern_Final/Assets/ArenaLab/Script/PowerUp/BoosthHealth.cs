using UnityEngine;

public class BoostHealthVisitor : IVisitor
{
    private readonly float bonusHealth;

    public BoostHealthVisitor(float bonusHealth)
    {
        this.bonusHealth = bonusHealth;
    }

    public void Visit(Player player)
    {
        player.MaxHealth += bonusHealth;
        player.RecordUpgrade($"Health +{bonusHealth:0.##}");

        Debug.Log($"Player MaxHealth +{bonusHealth}");
    }

    public void Visit(Drone drone)
    {
        float value = bonusHealth * 0.5f;
        drone.MaxHealth += value;
        drone.RecordUpgrade($"Health +{value:0.##}");

        Debug.Log("Drone health boosted");
    }

    public void Visit(Turret turret)
    {
        float value = bonusHealth * 1.5f;
        turret.MaxHealth += value;
        turret.RecordUpgrade($"Health +{value:0.##}");

        Debug.Log("Turret health boosted");
    }
}
