using System.Collections;
using UnityEngine;

public class DeadState : State<Monster>
{
    private Animator animator;

    private float delay = 2f;

    readonly private int deadHash = Animator.StringToHash("Dead");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnterState()
    {
        animator.SetBool(deadHash, true);
        context.StartCoroutine(DisableWithDelay());
    }

    public override void OnExitState()
    {
    }
    
    public override void Update(float deltaTime)
    {
    }

    private IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(delay);

        context.gameObject.SetActive(false);
    }
}
