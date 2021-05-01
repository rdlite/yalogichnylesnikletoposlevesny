using UnityEngine;

public class EnemyShooter : Enemy
{
    [SerializeField] private EnemyActingCounters _enemyActingCountersData;
    [SerializeField] private EnemyArrowData _enemyArrowData;

    [SerializeField] private Transform _startArrowPoint;

    protected override void InitBehaviours()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerComponentsManager>().transform;
        }

        _enemyAttackBehaviour = new EnemyAttackerOnPlaceBehaviour(target, transform, _startArrowPoint, _enemyStats, _enemyActingCountersData, _enemyArrowData);
    }
}
