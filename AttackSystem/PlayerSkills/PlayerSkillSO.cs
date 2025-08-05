using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Skill", menuName = "Skill System/New Player Skill")]
public abstract class PlayerSkillSO : ScriptableObject
{
    public virtual void Execute(PlayerController player, Vector3 direction)
    {
        Execute(player, direction, null);
    }
    public abstract void Execute(PlayerController player, Vector3 direction, Action onExitSkill);
}
