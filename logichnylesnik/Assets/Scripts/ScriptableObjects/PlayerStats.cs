using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float MoveSpeed;
    public float GetMoveSpeed() => MoveSpeed;

    [SerializeField] private float AttackSpeed;
    public float GetAttackSpeed() => AttackSpeed;
}
