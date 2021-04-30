using UnityEngine;
using UnityEngine.AI;

public class MovementToPlayerBehaviour : IEnemyMovable
{
    private Transform _target;
    private NavMeshAgent _navMeshAgent;

    public MovementToPlayerBehaviour(Transform target, NavMeshAgent navMeshAgent)
    {
        _target = target;
        _navMeshAgent = navMeshAgent;
    }

    public void MoveEnemy()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
