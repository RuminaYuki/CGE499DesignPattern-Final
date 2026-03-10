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

        Debug.Log($"Player MaxHealth +{bonusHealth}");
    }

    public void Visit(Drone drone)
    {
        drone.MaxHealth += bonusHealth * 0.5f;

        Debug.Log("Drone health boosted");
    }

    public void Visit(Turret turret)
    {
        turret.MaxHealth += bonusHealth * 1.5f;

        Debug.Log("Turret health boosted");
    }
}