using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterDB", menuName = "Monster/New MonsterDB")]
public class MonsterDBSO : ScriptableObject
{
    public MonsterSO[] monsters;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (monsters.Length != 0)
        {
            for (int i =0; i < monsters.Length; i++)
            {
                monsters[i].id = i;
            }
        }
    }
#endif
}
