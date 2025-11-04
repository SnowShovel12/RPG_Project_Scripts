using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public float speed = 0;
    [HideInInspector]
    public int damage = 0;
    [HideInInspector]
    public IAttackable attacker;
    [HideInInspector]
    public Transform target;

    public float lifetime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        IDamagable target = other.GetComponent<IDamagable>();

        if (target != null)
        {
            bool canAttack = attacker.IsPlayer || target.IsPlayer;
            if (!canAttack || target == attacker) return;

            if (target.IsAlive && canAttack)
            {
                target.TakeDamage(damage, attacker);
            }
        }

        Destroy(gameObject);
    }
    void Start()
    {
        if (target)
        {
            Vector3 dest = target.transform.position;
            dest.y += 0.5f;
            transform.LookAt(dest);
        }

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
