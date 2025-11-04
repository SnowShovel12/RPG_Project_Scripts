using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image staminaBar;
    [SerializeField]
    private TextMeshProUGUI currentHealth;
    [SerializeField]
    private TextMeshProUGUI maxHealth;
    [SerializeField]
    private TextMeshProUGUI currentStamina;
    [SerializeField]
    private TextMeshProUGUI maxStamina;

    public PlayerStatSO playerStat;

    private void Start()
    {
        healthBar.fillAmount = playerStat.HealthPercent;
        staminaBar.fillAmount = playerStat.StaminaPercent;
    }

    private void OnEnable()
    {
        playerStat.OnChangedStat += OnChangeStats;
    }
    private void OnDisable()
    {
        playerStat.OnChangedStat -= OnChangeStats;
    }
    private void OnChangeStats(PlayerStatSO playerStat)
    {
        healthBar.fillAmount = playerStat.HealthPercent;
        staminaBar.fillAmount = playerStat.StaminaPercent;
        currentHealth.text = playerStat.Health.ToString();
        currentStamina.text = ((int)playerStat.Stamina).ToString();
        maxHealth.text = playerStat.GetModifiedValue(StatType.Health).ToString();
        maxStamina.text = playerStat.GetModifiedValue(StatType.Stamina).ToString();
    }
}
