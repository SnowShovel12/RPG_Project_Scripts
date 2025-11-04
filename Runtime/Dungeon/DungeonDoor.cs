using UnityEngine;

public class DungeonDoor : MonoBehaviour, IInteractable
{
    public DungeonSO targetDungeon;
    public Vector3 spawnPositionInNextDungeon;

    public string InteractText => "Door";

    public void Interact()
    {
        MoveNextDungeon();
        InteractManager.Instance.RemoveInteractButton(this);
    }

    private void MoveNextDungeon()
    {
        DungeonManager.Instance.LoadDungeonWithFade(targetDungeon, spawnPositionInNextDungeon);
    }
}