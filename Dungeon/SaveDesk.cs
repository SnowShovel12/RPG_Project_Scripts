using UnityEngine;

public class SaveDesk : MonoBehaviour, IInteractable
{
    public string InteractText => "SaveDesk";

    public void Interact()
    {
        GameManager.Instance.HealPlayer();
        GameManager.Instance.ReviveAllMonsters();
    }
}
