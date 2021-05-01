using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyOnCollisionAttacker))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyStats _enemyStats;
    protected Transform target;

    [SerializeField] private GameObject _nearestIndicator;

    protected IEnemyMovable _enemyMovableBehaviour;
    protected IEnemyAttack _enemyAttackBehaviour;

    protected abstract void InitBehaviours();

    public void SetNearestIndicatorActive(bool value)
    {
        _nearestIndicator.SetActive(value);
    }

    public void InitEnemy(Transform target)
    {
        this.target = target;

        EnemyGlobalListener.Instance.AddEnemy(this);

        GetComponent<EnemyHealth>().InitHeath(_enemyStats.GetHealth());
        GetComponent<EnemyOnCollisionAttacker>().InitEnemyAttacker(_enemyStats.GetAttackDamage());
        GetComponent<NavMeshAgent>().speed = _enemyStats.GetMoveSpeed();

        InitBehaviours();

        StartCoroutine(CoroutineUpdate());
    }

    private void Move()
    {
        _enemyMovableBehaviour?.MoveEnemy();
    }

    private void Attack()
    {
        _enemyAttackBehaviour?.Attack();
    }

    private IEnumerator CoroutineUpdate()
    {
        while (gameObject.activeSelf)
        {
            Move();
            Attack();

            yield return null;
        }
    }

    private void OnDestroy()
    {
        EnemyGlobalListener.Instance.RemoveEnemy(this);
    }
}
