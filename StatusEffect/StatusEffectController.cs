using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffectController : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public IDamagable damagable;

    private void Start()
    {
        damagable = GetComponent<IDamagable>();
        activeEffects.Clear();
    }

    public void AddEffect(StatusEffectSO effect, float duration)
    {
        StatusEffect existing = activeEffects.Find(e => e.data == effect);

        if (existing != null)
        {
            if (existing.duration - existing.elapsed < duration)
            {
                existing.elapsed = 0f;
                existing.duration = duration;
                return;
            }
            else
            {
                return;
            }
        }

        StatusEffect newEffect = new StatusEffect(effect, duration);
        activeEffects.Add(newEffect);
        effect.Apply(damagable);
    }

    private void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect e = activeEffects[i];
            e.elapsed += Time.deltaTime;

            if (e.elapsed >= e.duration)
            {
                e.data.Remove(damagable);
                activeEffects.RemoveAt(i);
            }
        }
    }
}
