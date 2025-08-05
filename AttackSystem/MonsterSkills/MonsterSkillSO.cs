using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Skill", menuName = "Skill System/New Monster Skill")]
public abstract class MonsterSkillSO : ScriptableObject
{
    public virtual void Execute(Monster monster, Vector3 direction)
    {
        Execute(monster, direction, null);
    }
    public abstract void Execute(Monster monster, Vector3 direction, Action onExitSkill);
}
