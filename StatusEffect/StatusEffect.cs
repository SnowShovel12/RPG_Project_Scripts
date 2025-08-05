using UnityEngine;

public class StatusEffect
{
    public float duration;
    public float elapsed;
    public StatusEffectSO data;

    public StatusEffect(StatusEffectSO data, float duration)
    {
        this.data = data;
        this.duration = duration;
        elapsed = 0;
    }
}
