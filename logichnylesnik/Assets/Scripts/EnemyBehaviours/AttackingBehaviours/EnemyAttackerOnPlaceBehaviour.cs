using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackerOnPlaceBehaviour : IEnemyAttack
{
    private Transform _target, _thisBody;

    private EnemyArrowData _enemyArrowData;

    private Transform _startArrowPoint;

    private float _timerBeforeAct, _maxTimeToAct;
    private float _attackDamage;

    public EnemyAttackerOnPlaceBehaviour(Transform target, Transform thisEnemy, Transform startArrowPoint, EnemyStats enemyStats, EnemyActingCounters counters, EnemyArrowData enemyArrowData)
    {
        _target = target;
        _thisBody = thisEnemy;

        _maxTimeToAct = counters.MaxTimeToAct;

        _attackDamage = enemyStats.GetAttackDamage();
        _enemyArrowData = enemyArrowData;

        _startArrowPoint = startArrowPoint;
    }

    public void Attack()
    {
        _timerBeforeAct += Time.deltaTime;

        _thisBody.LookAt(_target.position);

        if (_timerBeforeAct >= _maxTimeToAct)
        {
            CreateArrow();

            _timerBeforeAct = 0;
        }
    }

    private void CreateArrow()
    {
        GameObject enemyArrow = _enemyArrowData.InstantiatePrefab();

        enemyArrow.transform.position = _startArrowPoint.position;
        enemyArrow.GetComponent<Arrow>().InitArrow(_target.position, _attackDamage);
    }
}
