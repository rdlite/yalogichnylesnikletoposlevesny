using UnityEngine;
using UnityEngine.AI;

public class EnemyFlying : Enemy
{
    [SerializeField] private EnemyActingCounters EnemyActingCountersData;

    protected override void InitBehaviours()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerComponentsManager>().transform;
        }

        _enemyMovableBehaviour = new FlyToPlayerBehaviour(target, transform, _enemyStats, EnemyActingCountersData);
    }
}
