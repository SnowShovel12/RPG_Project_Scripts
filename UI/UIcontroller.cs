using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    private GameObject _uis;
    private JoyStickController _joyStickController;
    private SkillButton[] _skillButtons;
    private AttackButton _attackButton;

    private void Awake()
    {
        _uis = GameManager.Instance.ui;
        _joyStickController = _uis.GetComponentInChildren<JoyStickController>();
        _skillButtons = _uis.GetComponentsInChildren<SkillButton>();
        _attackButton = _uis.GetComponentInChildren<AttackButton>();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);

        TurnUI(true);

        GameManager.Instance.ResumeGame();
    }
    

    public void OpenUI()
    {
        gameObject.SetActive(true);

        TurnUI(false);

        GameManager.Instance.PauseGame();
    }

    private void TurnUI(bool set)
    {
        _joyStickController.gameObject.SetActive(set);
        _attackButton.gameObject.SetActive(set);
        foreach (SkillButton skillButton in _skillButtons)
        {
            skillButton.gameObject.SetActive(set);
        }
    }
}
