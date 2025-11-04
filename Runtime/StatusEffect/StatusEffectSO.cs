using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    public abstract void Apply(IDamagable target);
    public abstract void Remove(IDamagable target);
}
