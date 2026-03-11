using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private TMP_Text enemyInfoTemplate;
    [SerializeField] private RectTransform enemyInfoParent;
    [SerializeField] private float enemyInfoSpacing = 55f;

    [Header("Enemy UI Limit")]
    [SerializeField] private int maxEnemyUI = 5;

    private Player player;
    private Vector2 startPos;
    private bool templateHidden;

    private readonly List<TMP_Text> enemyTextPool = new();

    private bool canUpdate;

    private void Awake()
    {
        if (playerInfoText == null)
        {
            Transform t = transform.Find("PlayerInfor");
            if (t != null)
                playerInfoText = t.GetComponent<TMP_Text>();
        }

        if (enemyInfoTemplate == null)
        {
            Transform t = transform.Find("EnemyInfor");
            if (t != null)
                enemyInfoTemplate = t.GetComponent<TMP_Text>();
        }

        if (enemyInfoTemplate != null)
        {
            startPos = enemyInfoTemplate.rectTransform.anchoredPosition;

            if (enemyInfoParent == null)
                enemyInfoParent = enemyInfoTemplate.transform.parent as RectTransform;
        }

        player = FindFirstObjectByType<Player>();
    }

    public void StartUpdate()
    {
        canUpdate = true;
    }

    public void StopUpdate()
    {
        canUpdate = false;
    }

    private void Update()
    {
        if(!canUpdate) return;
        UpdatePlayerUI(); 
        UpdateEnemyUI();
    }

    private void UpdatePlayerUI()
    {
        if (playerInfoText == null) return;

        if (player == null)
            player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            playerInfoText.text = "-Player : Not Found";
            return;
        }

        playerInfoText.text =
            $"-Player -Upgrade: {FormatUpgrades(player.Upgrades)}\n" +
            $"-Damage: {player.Damage:0.##} -Health: {player.CurrentHealth:0.##}/{player.MaxHealth:0.##} -Movement: {player.MovementSpeed:0.##}";
    }

    private void UpdateEnemyUI()
    {
        if (enemyInfoTemplate == null) return;

        if (!templateHidden)
        {
            enemyInfoTemplate.gameObject.SetActive(false);
            templateHidden = true;
        }

        List<MonoBehaviour> enemies = new();

        Drone[] drones = FindObjectsByType<Drone>(FindObjectsSortMode.None);
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsSortMode.None);

        foreach (Drone d in drones)
            enemies.Add(d);

        foreach (Turret t in turrets)
            enemies.Add(t);

        enemies.Sort((a, b) => a.GetInstanceID().CompareTo(b.GetInstanceID()));

        int startIndex = Mathf.Max(0, enemies.Count - maxEnemyUI);
        int visibleCount = enemies.Count - startIndex;

        while (enemyTextPool.Count < visibleCount)
        {
            TMP_Text newText = Instantiate(enemyInfoTemplate, enemyInfoParent);
            newText.gameObject.SetActive(true);
            enemyTextPool.Add(newText);
        }

        for (int i = 0; i < enemyTextPool.Count; i++)
        {
            enemyTextPool[i].gameObject.SetActive(i < visibleCount);
        }

        int uiIndex = 0;

        for (int i = startIndex; i < enemies.Count; i++)
        {
            MonoBehaviour enemy = enemies[i];
            TMP_Text text = enemyTextPool[uiIndex];

            int displayNumber = i + 1;

            if (enemy is Drone drone)
            {
                text.text =
                    $"-Enemy {displayNumber} : Drone -Upgrade: {FormatUpgrades(drone.Upgrades)}\n" +
                    $"-Damage: {drone.Damage:0.##} -Health: {drone.CurrentHealth:0.##}/{drone.MaxHealth:0.##} -Movement: {drone.MovementSpeed:0.##}";
            }
            else if (enemy is Turret turret)
            {
                text.text =
                    $"-Enemy {displayNumber} : Turret -Upgrade: {FormatUpgrades(turret.Upgrades)}\n" +
                    $"-Damage: {turret.Damage:0.##} -Health: {turret.CurrentHealth:0.##}/{turret.MaxHealth:0.##} -Movement: {turret.MovementSpeed:0.##}";
            }
            else
            {
                text.text = $"-Enemy {displayNumber} : Unknown";
            }

            text.rectTransform.anchoredPosition =
                startPos + new Vector2(0f, -enemyInfoSpacing * uiIndex);

            uiIndex++;
        }
    }

    private string FormatUpgrades(IReadOnlyList<string> upgrades)
    {
        if (upgrades == null || upgrades.Count == 0)
            return "None";

        return string.Join(", ", upgrades);
    }
}