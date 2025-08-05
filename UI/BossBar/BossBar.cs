using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private TextMeshProUGUI bossName;

    public BossBar Activate(float healthPercent, string name)
    {
        healthBar.fillAmount = healthPercent;
        bossName.text = name;
        gameObject.SetActive(true);

        return this;
    }

    public void Set(float healthPercent)
    {
        healthBar.fillAmount = healthPercent;
    }

    public void Remove()
    {
        gameObject.SetActive(false);
    }
}
