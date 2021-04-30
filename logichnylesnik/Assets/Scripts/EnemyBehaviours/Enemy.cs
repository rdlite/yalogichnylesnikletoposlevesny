using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttacker))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats _enemyStats;

    [SerializeField] private GameObject _nearestIndicator;

    protected IEnemyMovable _enemyMovableBehaviour;

    protected abstract void InitBehaviours();

    public void SetNearestIndicatorActive(bool value)
    {
        _nearestIndicator.SetActive(value);
    }

    private void Move()
    {
        _enemyMovableBehaviour.MoveEnemy();
    }

    private void Start()
    {
        EnemyGlobalListener.Instance.AddEnemy(this);

        GetComponent<EnemyHealth>().InitHeath(_enemyStats.GetHealth());
        GetComponent<EnemyAttacker>().InitEnemyAttacker(_enemyStats.GetAttackDamage());
        GetComponent<NavMeshAgent>().speed = _enemyStats.GetMoveSpeed();

        InitBehaviours();
    }

    private void Update()
    {
        Move();
    }

    private void OnDestroy()
    {
        EnemyGlobalListener.Instance.RemoveEnemy(this);
    }
}
