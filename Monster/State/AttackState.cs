using System.Collections;
using UnityEngine;

public class AttackState : State<Monster>
{
    private float _rotationSpeed = 20f;
    private float _threshold = 5f;
    private Coroutine _lookAndAttack;

    public override void OnInitialized()
    {
    }

    public override void OnEnterState()
    {
        if (_lookAndAttack != null)
        {
            context.StopCoroutine(_lookAndAttack);
        }
        _lookAndAttack = context.StartCoroutine(LookAndAttack());
    }

    public override void OnExitState()
    {
        if (_lookAndAttack != null)
        {
            context.StopCoroutine(_lookAndAttack);
            _lookAndAttack = null;
        }
        context.attackController.StopAttack();
    }

    public override void Update(float deltaTime)
    {

    }

    private IEnumerator LookAndAttack()
    {
        while (true)
        {
            if (context.Target == null)
            {
                yield break;
            }

            Vector3 direction = (context.Target.position - context.transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);

                float angle = Quaternion.Angle(context.transform.rotation, targetRotation);
                if (angle < _threshold)
                {
                    context.attackController.ExecuteAttack();
                    yield break;
                }
            }

            yield return null;
        }
    }
}
