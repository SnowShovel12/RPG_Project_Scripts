using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Attack",menuName = "Skill System/Player/DashAttack")]
public class PlayerDashAttack : PlayerSkillSO
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;

    private int defaultLayer = 0;
    private int dashLayer = 0;

    public override void Execute(PlayerController player, Vector3 direction, Action onExitAttack)
    {
        if (dashLayer == 0) dashLayer = LayerMask.NameToLayer("PlayerDash");
        if (defaultLayer == 0) defaultLayer = LayerMask.NameToLayer("Player");
        player.StartCoroutine(StartDash(player, direction, onExitAttack));
    }

    private IEnumerator StartDash(PlayerController player, Vector3 direction, Action onExitAttack)
    {
        player.gameObject.layer = dashLayer;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            CollisionFlags flags = player.controller.Move(dashSpeed * direction * Time.deltaTime);
            elapsed += Time.deltaTime;
            if ((flags & CollisionFlags.Sides) != 0)
            {
                break;
            }

            yield return null;
        }
        
        player.gameObject.layer = defaultLayer;

        onExitAttack?.Invoke();
    }
}
