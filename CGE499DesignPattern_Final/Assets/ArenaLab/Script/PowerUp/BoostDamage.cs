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
        Debug.Log($"Player Damage +{bonusDamage}");
    }

    public void Visit(Drone drone)
    {
        drone.Damage += bonusDamage * 0.75f;
        Debug.Log($"Drone Damage +{bonusDamage * 0.75f}");
    }

    public void Visit(Turret turret)
    {
        turret.Damage += bonusDamage * 1.25f;
        Debug.Log($"Turret Damage +{bonusDamage * 1.25f}");
    }
}