using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster/New Monster")]
public class MonsterSO : ScriptableObject
{
    public int id = -1;
    public string monsterName = "";
    public GameObject monsterPrefab;

    [SerializeField]
    private int health;
    public int Health => health;
    [SerializeField]
    private int damage;
    public int Damage => damage;
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;
}
