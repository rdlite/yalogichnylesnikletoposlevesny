using UnityEngine;
using UnityEngine.AI;

public class FlyToPlayerBehaviour : IEnemyMovable
{
    private Transform _target, _thisBody;

    private float _timerBeforeAct, _timerToAct, _actingTime, _maxTimeToAct;
    private float _currentMoveSpeedMultiplier, _moveSpeedMultiplier, _startMoveSpeed;

    private float _startYPosition;

    public FlyToPlayerBehaviour(Transform target, Transform thisEnemy, EnemyStats enemyStats, EnemyActingCounters counters)
    {
        _startMoveSpeed = enemyStats.GetMoveSpeed();

        _target = target;
        _thisBody = thisEnemy;

        _startYPosition = _thisBody.position.y + 1f;

        _actingTime = counters.ActingTime;
        _maxTimeToAct = counters.MaxTimeToAct;

        _currentMoveSpeedMultiplier = 1f;
        _moveSpeedMultiplier = counters.ModifyingValueMultiplier;
    }

    public void MoveEnemy()
    {
        if (_timerBeforeAct >= _maxTimeToAct)
        {
            _timerToAct += Time.deltaTime;

            _currentMoveSpeedMultiplier = _moveSpeedMultiplier;

            if ((_thisBody.position - _target.position).magnitude < .5f)
            {
                StopActing();
            }

            if (_timerToAct >= _actingTime)
            {
                StopActing();
            }
        }
        else
        {
            _timerBeforeAct += Time.deltaTime;
        }

        MoveTransformToTarget();
    }

    private void MoveTransformToTarget()
    {
        Vector3 moveTo = new Vector3(_target.position.x - _thisBody.position.x, 0f, _target.position.z - _thisBody.position.z);
        _thisBody.position += moveTo * _startMoveSpeed * _currentMoveSpeedMultiplier * Time.deltaTime;
        _thisBody.position = new Vector3(_thisBody.position.x, _startYPosition, _thisBody.position.z);
    }

    private void StopActing()
    {
        _currentMoveSpeedMultiplier = 1f;

        _timerToAct = 0;
        _timerBeforeAct = 0;
    }
}