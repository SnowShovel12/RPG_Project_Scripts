using System;
using UnityEngine;

[Serializable]
public class PlayerComboAttack : PlayerAttack
{
    private int combo;
    public PlayerMeleeAttack[] attacks;

    public override void OnInitialized()
    {
        base.OnInitialized();
        combo = 0;
        foreach (PlayerMeleeAttack attack in attacks)
        {
            attack.player = player;
            attack.OnInitialized();
            attack.coolTime = 0;
            attack.hasGlobalCoolTime = false;
        }
        animationIndex = attacks[combo].animationIndex;
    }

    public override void ExecuteAttack()
    {
        attacks[combo].ExecuteAttack();
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        attacks[combo].OnEnterAttack(attacker);
    }

    public override void OnExitAttack()
    {
        attacks[combo].OnExitAttack();
        combo++;
        if (combo >= attacks.Length)
        {
            combo = 0;
        }
        animationIndex = attacks[combo].animationIndex;
    }

    public void ResetCombo()
    {
        combo = 0;
        animationIndex = attacks[combo].animationIndex;
    }

    protected override int GetDamage(IAttackable attacker)
    {
        return 0;
    }
}
