using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkable : Enemy
{
    public Transform target;

    protected override void InitBehaviours()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerComponentsManager>().transform;
        }

        _enemyMovableBehaviour = new MovementToPlayerBehaviour(target, GetComponent<NavMeshAgent>());
    }
}
