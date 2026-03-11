using UnityEngine;

public class ApplyPowerUp : MonoBehaviour
{
    public void ApplyRandomPowerUpToEveryone()
    {
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour obj in allObjects)
        {
            if (obj is IVisitable visitable)
            {
                IVisitor randomPowerUp = GetRandomPowerUp();
                visitable.Accept(randomPowerUp);

                Debug.Log($"{obj.name} got {randomPowerUp.GetType().Name}");
            }
        }
    }

    private IVisitor GetRandomPowerUp()
    {
        int r = Random.Range(0, 3);

        switch (r)
        {
            case 0:
                return new BoostDamageVisitor(3f);

            case 1:
                return new BoostSpeedVisitor(5f);

            case 2:
                return new BoostHealthVisitor(5f);

            default:
                return new BoostDamageVisitor(3f);
        }
    }
}