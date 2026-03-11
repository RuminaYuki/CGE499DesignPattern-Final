using UnityEngine;

public class FacadeGame : MonoBehaviour
{
    ApplyPowerUp applyPowerUp;
    RandomEnemySpawner randomEnemySpawner;
    UICharacter uICharacter;
    PlayerSpawner playerSpawner;
    UIGameManager uIGameManager;
    void Awake()
    {
        applyPowerUp = GetComponent<ApplyPowerUp>();
        playerSpawner = GetComponent<PlayerSpawner>();
        randomEnemySpawner = GetComponent<RandomEnemySpawner>();
        uICharacter = GetComponent<UICharacter>();
        uIGameManager = GetComponent<UIGameManager>();
    }
    public void StartGame()
    {
        playerSpawner.SpawnPlayer();
        randomEnemySpawner.SpawnEnemies();
        applyPowerUp.ApplyRandomPowerUpToEveryone();
        uICharacter.StartUpdate();
        uIGameManager.SetReference();
    }

    public void ResetGame()
    {
        randomEnemySpawner.ClearEnemies();
        uICharacter.StopUpdate();
        uIGameManager.ClearReference();
    }
}
