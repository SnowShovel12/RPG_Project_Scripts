using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public void CloseUI()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }
}
