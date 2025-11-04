using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    public HitboxConfig hitboxConfig;
    public AttackIndicatorConfig attackIndicatorConfig;

    public void Setup(Transform attacker, HitboxConfig config)
    {
        hitboxConfig = config;
        transform.localScale = config.size;
        Vector3 attackerOffset = attacker.TransformPoint(config.offset);
        attackerOffset.y = 0.1f;
        transform.position = attackerOffset;
        transform.rotation = attacker.rotation;

        gameObject.SetActive(true);
    }

    public void Setup(Transform attacker, AttackIndicatorConfig config)
    {
        attackIndicatorConfig = config;
        transform.localScale = config.size;
        transform.position = attacker.TransformPoint(config.offset);
        transform.rotation = attacker.rotation;

        gameObject.SetActive(true);
    }

    public void Remove()
    {
       gameObject.SetActive(false);
    }
}
