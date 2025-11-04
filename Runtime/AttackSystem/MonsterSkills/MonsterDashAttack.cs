using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Attack", menuName = "Skill System/Monster/DashAttack")]
public class MonsterDashAttack : MonsterSkillSO
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;

    private int _defaultLayer = 0;
    private int _dashLayer = 0;

    public override void Execute(Monster monster, Vector3 direction, Action onExitAttack)
    {
        if (_dashLayer == 0) _dashLayer = LayerMask.NameToLayer("MonsterDash");
        if (_defaultLayer == 0) _defaultLayer = LayerMask.NameToLayer("Monster");
        monster.StartCoroutine(StartDash(monster, direction, onExitAttack));
    }

    private IEnumerator StartDash(Monster monster, Vector3 direction, Action onExitAttack)
    {
        monster.gameObject.layer = _dashLayer;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            if (monster.IsStunned)
            {
                break;
            }
            CollisionFlags flags = monster.controller.Move(dashSpeed * direction * Time.deltaTime);
            elapsed += Time.deltaTime;
            if ((flags & CollisionFlags.Sides) != 0)
            {
                break;
            }

            yield return null;
        }

        monster.gameObject.layer = _defaultLayer;

        onExitAttack?.Invoke();
    }
}
