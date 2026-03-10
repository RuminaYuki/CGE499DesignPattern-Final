using UnityEngine;

public class BoostSpeedVisitor : IVisitor
{
    private readonly float bonusSpeed;

    public BoostSpeedVisitor(float bonusSpeed)
    {
        this.bonusSpeed = bonusSpeed;
    }

    public void Visit(Player player)
    {
        player.MoveSpeed += bonusSpeed;
        Debug.Log($"Player Damage +{bonusSpeed}");
    }

    public void Visit(Drone drone)
    {
        drone.MoveSpeed += bonusSpeed * 0.8f;
        Debug.Log($"Drone Damage +{bonusSpeed * 0.75f}");
    }

    public void Visit(Turret turret)
    {
        turret.FireRate += bonusSpeed * 1.25f;
        Debug.Log($"Turret Damage +{bonusSpeed * 1.25f}");
    }
}
