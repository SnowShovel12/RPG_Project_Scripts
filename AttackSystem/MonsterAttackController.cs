using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class MonsterAttackController : MonoBehaviour
{
    [SerializeField]
    private float globalCoolTime = 1f;

    [SerializeField]
    private MonsterMeleeAttack[] meleeAttacks;
    [SerializeField]
    private MonsterRangeAttack[] rangeAttacks;
    [SerializeField]
    private MonsterSkill[] skills;

    private List<MonsterAttack> _attacks = new List<MonsterAttack>();

    private Monster _monster;
    private Animator _animator;
    private AttackIndicator _attackIndicator;

    private readonly int _attackHash = Animator.StringToHash("Attack");

    [HideInInspector]
    public Transform target;

    public MonsterAttack currentAttack;
    public bool CanAttack { get; private set; }

    private void Start()
    { 
        _monster = GetComponent<Monster>();
        _animator = GetComponent<Animator>();
        CanAttack = false;

        foreach(MonsterMeleeAttack meleeAttack in meleeAttacks)
        {
            meleeAttack.OnInitialized();
            meleeAttack.monster = _monster;
            _attacks.Add(meleeAttack);
        }
        foreach(MonsterRangeAttack rangeAttack in rangeAttacks)
        {
            rangeAttack.OnInitialized();
            rangeAttack.monster = _monster;
            _attacks.Add(rangeAttack);
        }
        foreach(MonsterSkill skill in skills)
        {
            skill.OnInitialized();
            skill.monster = _monster;
            _attacks.Add(skill);
        }
    }

    private void Update()
    {
        foreach (MonsterAttack attack in _attacks)
        {
            attack.Tick(Time.deltaTime);
        }

        target = _monster.Target;

        SelectCurrentAttack();
    }

    public void ExecuteAttack()
    {
        _monster.IsAttacking = true;
        Vector3 direction = _monster.transform.forward;
        direction.y = 0;
        currentAttack.SetDirection(direction);
        currentAttack.ExecuteAttack();
        StartCoolTime();
        GenerateAttackIndicator();
    }

    public void StopAttack()
    {
        _monster.IsAttacking = false;
        _animator.SetBool(_attackHash, false);
    }

    private void StartCoolTime()
    {
        currentAttack.StartCooltime();

        foreach(MonsterAttack attack in _attacks)
        {
            if (attack == currentAttack) continue;
            attack.AddGlobalCooltime(globalCoolTime);
        }
    }

    public void OnEnterAttack(int num)
    {
        RemoveAttackIndicator();
        if (num >= _attacks.Count || num < 0) return;
        _attacks[num].OnEnterAttack(_monster, target);
    }

    public void OnExitAttack(int num)
    {
        if (num >= _attacks.Count || num < 0) return;
        _attacks[num].OnExitAttack();
    }

    private void GenerateAttackIndicator()
    {
        if (currentAttack is MonsterMeleeAttack meleeAttack && meleeAttack.showAttackRange)
        {
            _attackIndicator = GameManager.Instance.hitboxPoolManager.Get<AttackIndicator>(0);
            _attackIndicator.Setup(_monster.Transform, meleeAttack.hitboxConfig);
        }
        else if (currentAttack is MonsterSkill skill)
        {
            _attackIndicator = GameManager.Instance.hitboxPoolManager.Get<AttackIndicator>(0);
            _attackIndicator.Setup(_monster.Transform, skill.indicatorConfig);
        }
    }

    public void RemoveAttackIndicator()
    {
        if(_attackIndicator != null)
        {
            _attackIndicator.Remove();
            _attackIndicator = null;
        }
    }

    private void SelectCurrentAttack()
    {
        if (_monster.IsAttacking || target == null)
        {
            CanAttack = false;
            return;
        }

        Vector3 attackerPos = Vector3.ProjectOnPlane(_monster.Transform.position, Vector3.up);
        Vector3 targetPos = Vector3.ProjectOnPlane(target.position, Vector3.up);
        float targetDistance = Vector3.Distance(attackerPos, targetPos);

        MonsterAttack best = null;
        MonsterAttack bestInDistance = null;
        foreach (MonsterAttack attack in _attacks)
        {
            if (!attack.IsReady) continue;

            if (best == null || attack.priority > best.priority)
            {
                best = attack;
            }

            if (targetDistance <= attack.range)
            {
                if (bestInDistance == null || attack.priority > bestInDistance.priority)
                {
                    bestInDistance = attack;
                }
            }
        }

        currentAttack = bestInDistance ?? best;

        CanAttack = currentAttack != null && targetDistance <= currentAttack.range;
    }

    #region drawGizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (MonsterMeleeAttack meleeAttack in meleeAttacks)
        {
            if (!meleeAttack.hitboxConfig.drawHitbox) continue;

            Vector3 worldPos = transform.TransformPoint(meleeAttack.hitboxConfig.offset);
            Vector3 worldSize = meleeAttack.hitboxConfig.size;

            Gizmos.DrawCube(worldPos, worldSize);
        }

        foreach (MonsterSkill skill in skills)
        {
            if (!skill.hitboxConfig.drawHitbox) continue;

            Vector3 worldPos = transform.TransformPoint(skill.hitboxConfig.offset);
            Vector3 worldSize = skill.hitboxConfig.size;

            Gizmos.DrawCube(worldPos, worldSize);
        }
    }
    #endregion drawGizmo
}
