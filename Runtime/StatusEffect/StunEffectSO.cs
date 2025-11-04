using UnityEngine;

[CreateAssetMenu(fileName = "StunEffect", menuName = "StatusEffect/StunEffect")]
public class StunEffectSO : StatusEffectSO
{
    public override void Apply(IDamagable target)
    {
        target.IsStunned = true;
        target.Animator.SetTrigger("StartStun");
        target.Animator.SetBool("Attack", false);
        //현재 몬스터만 기절 가능 추후 플레이어도 추가
        if (target is Monster monster)
        {
            //임시로 이름 변경으로 기절상태 표시
            monster.ChangeName("Stunned");
        }
    }

    public override void Remove(IDamagable target)
    {
        target.IsStunned = false;
        target.Animator.SetTrigger("EndStun");
        //현재 몬스터만 기절 가능 추후 플레이어도 추가
        if (target is Monster monster && monster.IsAlive)
        {
            monster.MakeIdleState();
            //임시로 이름 변경으로 기절상태 표시
            monster.ResetName();
        }
    }
}
