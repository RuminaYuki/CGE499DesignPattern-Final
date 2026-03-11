using UnityEngine;

public class BoostDamageVisitor : IVisitor
{
    private readonly float bonusDamage;

    public BoostDamageVisitor(float bonusDamage)
    {
        this.bonusDamage = bonusDamage;
    }

    public void Visit(Player player)
    {
        player.Damage += bonusDamage;
        player.RecordUpgrade($"Damage +{bonusDamage:0.##}");
        Debug.Log($"Player Damage +{bonusDamage}");
    }

    public void Visit(Drone drone)
    {
        float value = bonusDamage * 0.75f;
        drone.Damage += value;
        drone.RecordUpgrade($"Damage +{value:0.##}");
        Debug.Log($"Drone Damage +{value}");
    }

    public void Visit(Turret turret)
    {
        float value = bonusDamage * 1.25f;
        turret.Damage += value;
        turret.RecordUpgrade($"Damage +{value:0.##}");
        Debug.Log($"Turret Damage +{value}");
    }
}
