using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffectController : MonoBehaviour
{
    private List<StatusEffect> _activeEffects = new List<StatusEffect>();

    public IDamagable damagable;

    private void Start()
    {
        damagable = GetComponent<IDamagable>();
        _activeEffects.Clear();
    }

    public void AddEffect(StatusEffectSO effect, float duration)
    {
        StatusEffect existing = _activeEffects.Find(e => e.data == effect);

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
        _activeEffects.Add(newEffect);
        effect.Apply(damagable);
    }

    private void Update()
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect e = _activeEffects[i];
            e.elapsed += Time.deltaTime;

            if (e.elapsed >= e.duration)
            {
                e.data.Remove(damagable);
                _activeEffects.RemoveAt(i);
            }
        }
    }
}
