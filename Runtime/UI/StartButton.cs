using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
