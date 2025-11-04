using System;
using UnityEngine;

[Serializable]
public class PlayerComboAttack : PlayerAttack
{
    private int _combo;
    public PlayerMeleeAttack[] attacks;

    public override void OnInitialized()
    {
        base.OnInitialized();
        _combo = 0;
        foreach (PlayerMeleeAttack attack in attacks)
        {
            attack.player = player;
            attack.OnInitialized();
            attack.coolTime = 0;
            attack.hasGlobalCoolTime = false;
        }
        animationIndex = attacks[_combo].animationIndex;
    }

    public override void ExecuteAttack()
    {
        attacks[_combo].ExecuteAttack();
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        attacks[_combo].OnEnterAttack(attacker);
    }

    public override void OnExitAttack()
    {
        attacks[_combo].OnExitAttack();
        _combo++;
        if (_combo >= attacks.Length)
        {
            _combo = 0;
        }
        animationIndex = attacks[_combo].animationIndex;
    }

    public void ResetCombo()
    {
        _combo = 0;
        animationIndex = attacks[_combo].animationIndex;
    }

    protected override int GetDamage(IAttackable attacker)
    {
        return 0;
    }
}
