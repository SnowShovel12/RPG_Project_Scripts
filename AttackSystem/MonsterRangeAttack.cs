using System;
using UnityEngine;

[Serializable]
public class MonsterRangeAttack : MonsterAttack
{
    public GameObject projectilePrefab;
    public Transform startPosition;
    public float projectileSpeed;

    public override void OnInitialized()
    {
        base.OnInitialized();
    }

    public override void ExecuteAttack()
    {
        monster.animator.SetBool(attackHash, true);
        monster.animator.SetInteger(attackIndexHash, animationIndex);
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        Vector3 projectilePosition = startPosition?.position ?? attacker.Transform.position;
        GameObject projectileGO = GameObject.Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
        projectileGO.transform.forward = attacker.Transform.forward;
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.speed = projectileSpeed;
            projectile.attacker = attacker;
            projectile.target = target;
            projectile.damage = GetDamage(attacker);
        }
    }

    protected override int GetDamage(IAttackable attacker)
    {
        return (int)Math.Round(attacker.Damage * modifier);
    }

    public override void OnExitAttack()
    {

    }
}
