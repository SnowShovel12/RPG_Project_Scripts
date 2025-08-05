using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Parry", menuName = "Skill System/Player/Parry")]
public class PlayerParry : PlayerSkillSO
{
    public float duration;
    public float stunDuration;
    public StatusEffectSO stunEffect;
    public StatusEffectSO immuneEffect;

    public override void Execute(PlayerController player, Vector3 direction, Action onExitSkill)
    {
        player.StartCoroutine(StartParry(player));
    }

    private void ParryToAttacker(IAttackable attacker)
    {
        if (attacker is IDamagable damagable)
        {
            damagable.ApplyStatusEffect(stunEffect, stunDuration);
        }
    }

    private IEnumerator StartParry(PlayerController player)
    {
        player.OnTakeDamage += ParryToAttacker;
        player.ApplyStatusEffect(immuneEffect, duration);

        yield return new WaitForSeconds(duration);

        player.OnTakeDamage -= ParryToAttacker;
    }
}
