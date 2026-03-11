using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UIGameManager : MonoBehaviour
{
    Player player;
    private List<IEnemy> enemies = new();

    public GameObject canvasGroup;
    public TextMeshProUGUI GameOverOrWin;

    private int enemyCount;

    // Update is called once per frame
    void Update()
    {
        
    }  

    public void ClearReference()
    {
        player.Hs.OnDied -= GameOver;
        
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.Hs.OnDied -= GameWin;
        }

        enemies.Clear();
    }

    private void GameOver()
    {
        canvasGroup.SetActive(true);
        GameOverOrWin.text = "Gameover";
    }

    public void SetReference()
    {
        player = FindAnyObjectByType<Player>();
        player.Hs.OnDied += GameOver;

        enemyCount = 0;
        enemies.Clear();

        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var obj in allObjects)
        {
            if (obj is IEnemy enemy)
                {
                    enemy.Hs.OnDied += GameWin;
                    enemies.Add(enemy);
                    enemyCount++;
                }
        }
    }

    void GameWin()
    {
        enemyCount--;

        if(enemyCount <= 0)
        {
            canvasGroup.SetActive(true);
            GameOverOrWin.text = "GameWin";
        }
    }
}
