using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamagable, IAttackable
{
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public Animator animator;
    public JoyStickController joystick;
    public PlayerStatSO playerStat;
    public InventorySO inventory;
    [HideInInspector]
    public PlayerAttackController attackController;
    [HideInInspector]
    public StatusEffectController statusEffectController;

    public Action<IAttackable> OnTakeDamage;

    private readonly int velocityHash = Animator.StringToHash("Velocity");

    public float velocity = 5f;
    public float staminaRecoverCooltime = 2f;


    private Coroutine pauseRecoverStamina;

    [HideInInspector]
    public bool isAttacking = false;
    [HideInInspector]
    public bool canRecoverStamina = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<PlayerAttackController>();
        statusEffectController = GetComponent<StatusEffectController>();

        IsStunned = false;
        IsImmune = false;
    }

    void Update()
    {
        if (IsStunned) return;

        PlayerMove(Time.deltaTime);

        RecoverStamina(Time.deltaTime);

        //For test with moving
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAttack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attackController.ExecuteAttack(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GroundItem item = other.GetComponent<GroundItem>();
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (item)
        {
            if (!item.CanPickUp) return;
            if (inventory.AddItem(item.itemObject.data, 1)) Destroy(other.gameObject);
        }

        if (interactable != null)
        {
            InteractManager.Instance.AddInteractButton(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            InteractManager.Instance.RemoveInteractButton(interactable);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(Physics.gravity * Time.deltaTime);
    }

    private void PlayerMove(float deltaTime)
    {
        if (joystick == null) return;
        if (isAttacking) return;

        LookHandleDirection();
        Vector3 input = new Vector3(joystick.InputVector.x, 0, joystick.InputVector.y);
        if (input.magnitude < 0.1)
        {
            controller.Move(Vector3.zero);
            animator.SetFloat(velocityHash, 0);
        }

        controller.Move(transform.forward * input.magnitude * velocity * deltaTime);
        animator.SetFloat(velocityHash, input.magnitude);
    }

    public void LookHandleDirection()
    {
        Vector3 input = new Vector3(joystick.InputVector.x, 0, joystick.InputVector.y);
        if (input == Vector3.zero) return;
        transform.rotation = Quaternion.LookRotation(input);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), 0.5f);
    }

    public void StartAttack()
    {
        attackController.ExecuteAttack(0);
    }

    public void UseStamina(int value)
    {
        playerStat.AddStamina(-value);

        if (pauseRecoverStamina != null)
        {
            StopCoroutine(pauseRecoverStamina);
        }
        
        pauseRecoverStamina = StartCoroutine(PauseRecoverStamina(staminaRecoverCooltime));
    }

    IEnumerator PauseRecoverStamina(float delay)
    {
        canRecoverStamina = false;
        yield return new WaitForSeconds(delay);
        canRecoverStamina = true;
    }

    private void RecoverStamina(float deltaTime)
    {
        if (!canRecoverStamina) return;
        playerStat.AddStamina(20 * deltaTime);
    }

    public void WarpTo(Vector3 position)
    {
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = position;
            controller.enabled = true;
        }
    }
    #region IDamagable
    public bool IsAlive => playerStat.Health > 0;
    public bool IsStunned {  get; set; }
    public bool IsImmune { get; set; }
    public Animator Animator => animator;
    public void TakeDamage(int damage, IAttackable attacker, StatusEffectSO statusEffect = null, float duration = 0)
    {
        if (!IsAlive) return;
        OnTakeDamage?.Invoke(attacker);

        if (!IsImmune)
        {
            if (statusEffect != null)
            {
                statusEffectController?.AddEffect(statusEffect, duration);
            }
            int defence = playerStat.GetModifiedValue(StatType.Defense);
            int reducedDamage = (int)Math.Round(damage * (1 - (float)defence / (defence + 100)));
            playerStat.AddHealth(-reducedDamage);
        }
    }

    public void ApplyStatusEffect(StatusEffectSO statusEffect, float duraion)
    {
        statusEffectController?.AddEffect(statusEffect, duraion);
    }

    //Not used
    public float GetHealthPercent()
    {
        return 0;
    }
    #endregion IDamagable

    #region IAttackable
    public int Damage => playerStat.GetModifiedValue(StatType.Strengh);
    public bool IsPlayer => true;
    public Transform Transform => transform;
    public Transform Target => null;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    #endregion IAttackable
}
