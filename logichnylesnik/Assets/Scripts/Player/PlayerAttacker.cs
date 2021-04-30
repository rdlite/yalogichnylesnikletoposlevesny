using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private float _attackSpeed;
    private float _attackCounter;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private GameObject _shootPoint;

    public void SetAttackSpeed(float value)
    {
        _attackSpeed = value;
    }

    public void AttackingEvent()
    {
        _attackCounter += Time.deltaTime;

        if (_attackCounter >= _attackSpeed)
        {
            _attackCounter = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject newArrow = Instantiate(_arrowPrefab);

        newArrow.transform.position = _shootPoint.transform.position;

        newArrow.GetComponent<Arrow>().InitArrow(EnemyGlobalListener.Instance.NearestEnemy.transform.position);
    }
}