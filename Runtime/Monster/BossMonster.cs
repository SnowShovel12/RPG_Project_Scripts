using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossMonster : Monster
{
    private FieldOfView _fov;
    private bool _isPaused = false;
    private BossBar _bossBar;

    public float cameraDuration = 3f;
    public CinemachineCamera Camera;

    private Coroutine _bossIntro;

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(new DontMoveIdleState());
        _fov = GetComponent<FieldOfView>();
    }

    protected override void Start()
    {
        base.Start();
        MakeIdleState();
    }

    protected override void Update()
    {
        if (_isPaused) return;
        base.Update();
        if (target == null)
        {
            target = _fov.NearestTarget;
            if (target != null)
            {
                _bossIntro = StartCoroutine(PlayBossIntro());
            }
        }
    }

    public override void TakeDamage(int damage, IAttackable attacker, StatusEffectSO statusEffect = null, float duration = 0)
    {
        if (!IsAlive) return;
        if (_bossBar != null)
        {
            _bossBar.Set(GetHealthPercent());
        }
        base.TakeDamage(damage, attacker, statusEffect, duration);
    }

    private void OnDestroy()
    {
        if (_bossBar != null)
        {
            _bossBar.Remove();
            _bossBar = null;
        }
    }

    private void OnDisable()
    {
        if (_bossBar != null)
        {
            _bossBar.Remove();
            _bossBar = null;
        }

        if (_bossIntro != null)
        {
            StopCoroutine(_bossIntro);
        }
    }

    protected override void OnDead()
    {
        base.OnDead();
        if (_bossBar != null)
        {
            _bossBar.Remove();
        }
    }

    public override void MakeIdleState()
    {
        stateMachine.ChangeState<DontMoveIdleState>();
    }

    private IEnumerator PlayBossIntro()
    {
        GameManager.Instance.TurnUI(false);
        Camera.Priority = 20;
        _isPaused = true;

        yield return new WaitForSeconds(cameraDuration);

        GameManager.Instance.TurnUI(true);
        Camera.Priority = 0;
        _isPaused = false;

        _bossBar = GameManager.Instance.bossBar.Activate(GetHealthPercent(), data.monsterName);
        stateMachine.ChangeState<MoveState>();
    }
}
