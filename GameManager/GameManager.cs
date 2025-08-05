using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private DungeonDBSO dungeonDB;
    [SerializeField]
    private PlayerStatSO playerStat;

    public BossBar bossBar;
    public PoolManager hitboxPoolManager;
    public MonsterPoolManager monsterPoolManager;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ReviveAllMonsters()
    {
        dungeonDB.ReviveAllMonsters();
    }

    public void HealPlayer()
    {
        playerStat.HealPlayer();
    }
}
