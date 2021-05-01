using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float MoveSpeed;
    public float GetMoveSpeed() => MoveSpeed;

    [SerializeField] private float Health;
    public float GetHealth() => Health;

    [SerializeField] private float AttackDamage;
    public float GetAttackDamage() => AttackDamage;
}
