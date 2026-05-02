using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBreakoutManager : MonoBehaviour
{
    public GameObject uiWinGroup;
    public TextMeshProUGUI textWin;
    public BallController ball;

    private int brickCount;
    void Awake()
    {
        Time.timeScale = 0;
    }
    void OnEnable()
    {
        ball.OnDie += GameOver;
        Brick.OnAnyBrickDestroyed += GameWin;
    }
    void OnDisable()
    {
       ball.OnDie -= GameOver;
       Brick.OnAnyBrickDestroyed -= GameWin;
    }
    void Start()
    {
        brickCount = FindObjectsByType<Brick>(FindObjectsSortMode.None).Length;
    }
    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiWinGroup.SetActive(true);
        textWin.text = "GameOver";
    }
    void GameWin()
    {
        brickCount--;

        if (brickCount <= 0)
        {
            Time.timeScale = 0;
            uiWinGroup.SetActive(true);
            textWin.text = "GameWin";
        }
    }

    public void ChangeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
