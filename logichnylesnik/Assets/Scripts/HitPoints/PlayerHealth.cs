using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    private float _currentHealth = 100f;
    [SerializeField] private GameObject _healthBarCanvasPrefab;
    [SerializeField] private Transform _healthBarCanvasPoint;

    private GameObject _healthBar;

    public void InitHeath(float value)
    {
        _currentHealth = value;

        _healthBar = Instantiate(_healthBarCanvasPrefab);
        _healthBar.transform.position = _healthBarCanvasPoint.position;

        _healthBar.GetComponent<HealthBarCanvas>().InitSliderValues(_currentHealth);
        _healthBar.GetComponent<HealthBarCanvas>().FollowBy(_healthBarCanvasPoint);
    }

    public void ApplyDamage(float damageValue)
    {
        _currentHealth -= damageValue;

        _healthBar.GetComponent<HealthBarCanvas>().SetSliderValues(_currentHealth);

        if (_currentHealth <= 0)
        {
            print("ÄÀÉ ÄÀÉ ÄÀÉ ÀÌÉ ÄÀÐËÈÍ");
        }
    }
}
