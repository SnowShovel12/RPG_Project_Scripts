using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private DungeonDBSO dungeonDB;
    [SerializeField]
    private PlayerStatSO playerStat;

    public GameObject ui;
    public BossBar bossBar;
    public PoolManager hitboxPoolManager;
    public MonsterPoolManager monsterPoolManager;
    public PoolManager textPoolManager;

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void TogglePause()
    {
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void TurnUI(bool set)
    {
        ui.SetActive(set);
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
