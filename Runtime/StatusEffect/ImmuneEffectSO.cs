using UnityEngine;

[CreateAssetMenu(fileName = "ImmuneEffect", menuName = "StatusEffect/ImmuneEffect")]
public class ImmuneEffectSO : StatusEffectSO
{
    public override void Apply(IDamagable target)
    {
        target.IsImmune = true;
    }

    public override void Remove(IDamagable target)
    {
        target.IsImmune = false;
    }
}
