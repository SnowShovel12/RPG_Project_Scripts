using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterName;
    [SerializeField]
    private Image healthBar;

    public float HealthPercent
    {
        get => healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount;
        set
        {
            healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = value;
        }
    }

    public string MonsterName
    {
        get => monsterName.text;
        set
        {
            monsterName.text = value;
        }
    }

    public void CreateDamageText(string text)
    {
        DamageText damageText = GameManager.Instance.textPoolManager.Get<DamageText>(0, transform);
        damageText.Set(text);
    }
}
