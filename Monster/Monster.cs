using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IDamagable, IAttackable
{
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public MonsterAttackController attackController;
    [HideInInspector]
    public StatusEffectController statusEffectController;
    [HideInInspector]
    public MobSpawner mobSpawner;

    public MonsterSO data;
    public StateMachine<Monster> stateMachine;
    [SerializeField]
    private MonsterUI monsterUI;

    public DropTable dropTable;

    public int CurrentHealth { get; protected set; }
    public string Name => data.monsterName;
    public int Damage => data.Damage;
    public float AttackRange => CurrentAttack.range;
    public float MoveSpeed => data.MoveSpeed;

    public bool IsPlayer => false;
    public Transform Transform => transform;

    [SerializeField]
    protected Transform target;
    public Transform Target => target;

    [HideInInspector]
    public bool isAttacking = false;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }
    public MonsterAttack CurrentAttack => attackController.currentAttack;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
        attackController = GetComponent<MonsterAttackController>();
        statusEffectController = GetComponent<StatusEffectController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stateMachine = new StateMachine<Monster>(this, new DeadState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
        stateMachine.AddState(new RandomMoveState());
    }

    protected virtual void Start()
    {
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    protected virtual void Update()
    {
        if (IsStunned) return;
        stateMachine.Update(Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
        controller.Move(Physics.gravity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        attackController.RemoveAttackIndicator();
    }

    public void Set(MobSpawner mobSpawner, Vector3 spawnPoint, Quaternion lookDirection)
    {
        this.mobSpawner = mobSpawner;
        dropTable = mobSpawner.dropTable;
        data = mobSpawner.monsterObject;
        name = mobSpawner.monsterObject.monsterName;
        controller.enabled = false;
        transform.position = spawnPoint;
        transform.rotation = lookDirection;
        controller.enabled = true;

        CurrentHealth = data.Health;
        if (monsterUI)
        {
            monsterUI.MonsterName = name;
            monsterUI.HealthPercent = GetHealthPercent();
        }

        agent.speed = MoveSpeed;

        IsStunned = false;
        IsImmune = false;

        target = null;

        MakeIdleState();
    }

    #region IDamagable
    public bool IsAlive => CurrentHealth > 0;

    public bool IsStunned { get; set; }
    public bool IsImmune { get; set; }
    public Animator Animator => animator;

    public abstract void MakeIdleState();

    public virtual void TakeDamage(int damage, IAttackable attacker, StatusEffectSO statusEffect = null, float duration = 0)
    {
        if (!IsAlive) return;

        if (!IsImmune)
        {
            CurrentHealth -= damage;

            if (statusEffect != null && IsAlive)
            {
                ApplyStatusEffect(statusEffect, duration);
            }
        }

        if (monsterUI)
        {
            monsterUI.HealthPercent = GetHealthPercent();
        }

        if (!IsAlive)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        stateMachine.ChangeState<DeadState>();
        dropTable.SpawnItem(this);
        mobSpawner.canSpawn = false;
        attackController.RemoveAttackIndicator();
    }

    public void ApplyStatusEffect(StatusEffectSO statusEffect, float duraion)
    {
        statusEffectController?.AddEffect(statusEffect, duraion);
        if (statusEffect is StunEffectSO)
        {
            attackController.RemoveAttackIndicator();
        }
    }

    public float GetHealthPercent()
    {
        return (float)CurrentHealth / data.Health;
    }
    #endregion IDamagable
}