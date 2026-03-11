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
        player.Mv.Speed += bonusSpeed;

        player.RecordUpgrade($"Movement +{bonusSpeed:0.##}");

        Debug.Log($"Player Movement +{bonusSpeed:0.##}");
    }

    public void Visit(Drone drone)
    {
        float value = bonusSpeed * 0.8f;

        drone.Mv.Speed += value;

        drone.RecordUpgrade($"Movement +{value:0.##}");

        Debug.Log($"Drone Movement +{value:0.##}");
    }

    public void Visit(Turret turret)
    {
        float value = bonusSpeed;

        turret.FireRate = Mathf.Max(0.5f, turret.FireRate - value);

        turret.RecordUpgrade($"FireRate +{value:0.##}");

        Debug.Log($"Turret FireRate +{value:0.##}");
    }
}