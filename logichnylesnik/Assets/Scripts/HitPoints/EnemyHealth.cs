using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyHealth : MonoBehaviour, IDamagable, IPushable
{
    private float _currentHealth = 100f;
    [SerializeField] private GameObject _healthBarCanvasPrefab;
    [SerializeField] private Transform _healthBarCanvasPoint;

    private GameObject _healthBar;

    public event Action OnAttacked;
    public event Action OnDestroy;

    private bool _isDead = false;

    public void InitHeath(float value)
    {
        _currentHealth = value;

        _healthBar = Instantiate(_healthBarCanvasPrefab);
        _healthBar.transform.position = _healthBarCanvasPoint.position;

        _healthBar.GetComponent<HealthBarCanvas>().InitSliderValues(_currentHealth, _currentHealth);
        _healthBar.GetComponent<HealthBarCanvas>().FollowBy(_healthBarCanvasPoint);
    }

    public void ApplyDamage(float damageValue)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealth -= damageValue;

        _healthBar.GetComponent<HealthBarCanvas>().SetSliderValues(_currentHealth);

        if (_currentHealth <= 0)
        {
            _isDead = true;

            Destroy(_healthBar);

            OnDestroy?.Invoke();
            Destroy(gameObject, .2f);
        }

        OnAttacked?.Invoke();
    }

    public void PushAgainst(Vector3 againstPosition, float pushForce)
    {
        GetComponent<Rigidbody>().AddForce((againstPosition - transform.position).normalized * pushForce, ForceMode.Impulse);
    }
}
