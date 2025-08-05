using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerAttackController : MonoBehaviour
{
    private AttackIndicator _attackinIndicator;

    [SerializeField]
    private float globalCoolTime = 1f;

    public PlayerComboAttack[] comboAttacks;
    public PlayerSkill[] skills;

    [HideInInspector]
    public List<PlayerAttack> attacks = new List<PlayerAttack>();

    private PlayerController player;
    private Animator animator;

    readonly private int attackHash = Animator.StringToHash("Attack");

    public bool CanAttack { get; private set; }

    private void Start()
    { 
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        CanAttack = true;

        attacks.Clear();

        foreach(PlayerComboAttack comboAttack in comboAttacks)
        {
            comboAttack.player = player;
            comboAttack.OnInitialized();
            attacks.Add(comboAttack);
        }
        foreach (PlayerSkill skill in skills)
        {
            skill.player = player;
            skill.OnInitialized();
            attacks.Add(skill);
        }
    }

    private void Update()
    {
        foreach (PlayerAttack attack in attacks)
        {
            attack.Tick(Time.deltaTime);
        }
    }

    public void ExecuteAttack(int attackIndex)
    {
        ExecuteAttack(attackIndex, player.transform.forward);
    }

    public void ExecuteAttack(int attackIndex, Vector3 direction)
    {
        if (attackIndex >= attacks.Count || attackIndex < 0) return;
        PlayerAttack attack = attacks[attackIndex];
        if (!attack.IsReady || !CanAttack || player.playerStat.Stamina < attack.cost) return;
        player.IsAttacking = true;

        attack.SetDirection(direction);
        attack.ExecuteAttack();

        player.UseStamina(attack.cost);

        CanAttack = false;
        StartCoolTime(attack);
    }

    public void StopAttack()
    {
        player.IsAttacking = false;
        animator.SetBool(attackHash, false);
        CanAttack = true;
        foreach (PlayerComboAttack comboAttack in comboAttacks)
        {
            comboAttack.ResetCombo();
        }
    }

    private void StartCoolTime(PlayerAttack currentAttack)
    {
        currentAttack.StartCooltime();

        foreach(PlayerAttack attack in attacks)
        {
            if (attack == currentAttack) continue;
            attack.AddGlobalCooltime(globalCoolTime);
        }
    }

    public void OnEnterAttack(int num)
    {
        if (num >= attacks.Count || num < 0) return;
        attacks[num].OnEnterAttack(player);
    }

    public void OnExitAttack(int num)
    {
        if (num >= attacks.Count || num < 0) return;
        attacks[num].OnExitAttack();
    }

    public void EnableAttack()
    {
        CanAttack = true;
    }

    public float GetCooltimeRatio(int attackIndex)
    {
        return attacks[attackIndex].CooltimeRatio;
    }

    public float GetCurrentCooltime(int attackIndex)
    {
        return attacks[attackIndex].CurrentCooltime;
    }

    #region AttackDirectionIndicator
    public void DrawAttackDirection(int attackindex)
    {
        if (attacks[attackindex] is PlayerSkill skill)
        {
            _attackinIndicator = GameManager.Instance.hitboxPoolManager.Get<AttackIndicator>(0);
            _attackinIndicator.Setup(player.transform, skill.indicatorConfig);
        }
    }

    public void UpdateAttackDirection(Vector3 direction)
    {
        if (_attackinIndicator)
        {
            float offsetz = _attackinIndicator.attackIndicatorConfig.offset.z;
            _attackinIndicator.transform.position = player.transform.position + direction.normalized * offsetz;
            _attackinIndicator.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void RemoveAttackDirection()
    {
        if (_attackinIndicator != null)
        {
            _attackinIndicator.Remove();
            _attackinIndicator = null;
        }
    }
    #endregion
    #region drawGizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (PlayerComboAttack comboAttack in comboAttacks)
        {
            foreach (PlayerMeleeAttack meleeAttack in comboAttack.attacks)
            {
                if (!meleeAttack.hitboxConfig.drawHitbox) continue;

                Vector3 worldPos = transform.TransformPoint(meleeAttack.hitboxConfig.offset);
                Vector3 worldSize = meleeAttack.hitboxConfig.size;

                Gizmos.DrawCube(worldPos, worldSize);
            }
        }

        foreach (PlayerSkill skill in skills)
        {
            if (!skill.hitboxConfig.drawHitbox) continue;

            Vector3 worldPos = transform.TransformPoint(skill.hitboxConfig.offset);
            Vector3 worldSize = skill.hitboxConfig.size;

            Gizmos.DrawCube(worldPos, worldSize);
        }
    }
    #endregion drawGizmo
}
