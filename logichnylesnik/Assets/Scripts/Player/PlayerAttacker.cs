using System;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private float _attackSpeed;
    private float _attackDamage;
    private float _attackCounter;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private GameObject _shootPoint;

    public event Action OnShoot;

    public AttackType CurrentAttackType = AttackType.Default;
    public void ChangePlayerAttackType(int id)
    {
        CurrentAttackType = (AttackType)id;
    }

    private void Start()
    {
        OnShoot += Shoot;
    }

    public void SetAttackSpeed(float value)
    {
        _attackSpeed = value;
    }

    public void SetAttackDamage(float value)
    {
        _attackDamage = value;
    }

    public void AttackingEvent()
    {
        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackSpeed)
        {
            _attackCounter = 0;

            if (EnemyGlobalListener.Instance.NearestEnemy != null)
            {
                OnShoot.Invoke();
            }
        }
    }

    private void Shoot()
    {
        if (CurrentAttackType == AttackType.Default)
        {
            DefaultShot();
        }
        else if (CurrentAttackType == AttackType.Double)
        {
            DoubleShot();
        }
        else if (CurrentAttackType == AttackType.Triple)
        {
            TripleShot();
        }
    }

    private void DefaultShot()
    {
        InstantiateArrow(
            _arrowPrefab, 
            _shootPoint.transform.position, 
            EnemyGlobalListener.Instance.NearestEnemy.transform.position, 
            _attackDamage);
    }

    private void DoubleShot()
    {
        float arrowDistBetween = 6f;

        InstantiateArrow(
            _arrowPrefab,
            _shootPoint.transform.position + transform.right / arrowDistBetween,
            _shootPoint.transform.position + transform.right / arrowDistBetween + transform.forward,
            _attackDamage);

        InstantiateArrow(
            _arrowPrefab,
            _shootPoint.transform.position - transform.right / arrowDistBetween,
            _shootPoint.transform.position - transform.right / arrowDistBetween + transform.forward,
            _attackDamage);
    }

    private void TripleShot()
    {
        InstantiateArrow(
            _arrowPrefab,
            _shootPoint.transform.position,
            _shootPoint.transform.position + transform.forward,
            _attackDamage);

        InstantiateArrow(
            _arrowPrefab,
            _shootPoint.transform.position,
            _shootPoint.transform.position + transform.right,
            _attackDamage);

        InstantiateArrow(
            _arrowPrefab,
            _shootPoint.transform.position,
            _shootPoint.transform.position - transform.right,
            _attackDamage);
    }

    private void InstantiateArrow(GameObject arrow, Vector3 startPos, Vector3 target, float damage)
    {
        GameObject newArrow = Instantiate(arrow);
        newArrow.transform.position = startPos;
        Vector3 dirTarget = target;

        newArrow.GetComponent<Arrow>().InitArrow(GetDirectionToTarget(startPos, dirTarget), damage);
    }

    private Vector3 GetDirectionToTarget(Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }
}

public enum AttackType { Default = 0, Double = 1, Triple = 2 }