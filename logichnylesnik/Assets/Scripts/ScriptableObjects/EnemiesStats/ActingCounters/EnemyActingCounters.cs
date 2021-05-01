using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActingCounters")]
public class EnemyActingCounters : ScriptableObject
{
    public float MaxTimeToAct;
    public float ActingTime;
    public float ModifyingValueMultiplier;
}
