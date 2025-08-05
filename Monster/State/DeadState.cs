using System.Collections;
using UnityEngine;

public class DeadState : State<Monster>
{
    private Animator animator;

    private float _delay = 2f;

    private readonly int _deadHash = Animator.StringToHash("Dead");

    public override void OnInitialized()
    {
        animator = _context.GetComponent<Animator>();
    }

    public override void OnEnterState()
    {
        animator.SetBool(_deadHash, true);
        _context.StartCoroutine(DisableWithDelay());
    }

    public override void OnExitState()
    {
    }
    
    public override void Update(float deltaTime)
    {
    }

    private IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(_delay);

        _context.gameObject.SetActive(false);
    }
}
