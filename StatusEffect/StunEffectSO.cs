using UnityEngine;

[CreateAssetMenu(fileName = "StunEffect", menuName = "StatusEffect/StunEffect")]
public class StunEffectSO : StatusEffectSO
{
    public override void Apply(IDamagable target)
    {
        target.IsStunned = true;
        target.Animator.SetTrigger("StartStun");
        target.Animator.SetBool("Attack", false);
    }

    public override void Remove(IDamagable target)
    {
        target.IsStunned = false;
        target.Animator.SetTrigger("EndStun");
        if (target is Monster monster && monster.IsAlive)
        {
            monster.MakeIdleState();
        }
    }
}
