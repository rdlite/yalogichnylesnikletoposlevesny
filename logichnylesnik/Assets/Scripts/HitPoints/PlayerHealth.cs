using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    private float _currentHealth;
    [SerializeField] private GameObject _healthBarCanvasPrefab;
    [SerializeField] private Transform _healthBarCanvasPoint;

    private GameObject _healthBar;

    public void InitHeath(float value, float maxValue)
    {
        _currentHealth = value;

        _healthBar = Instantiate(_healthBarCanvasPrefab);
        _healthBar.transform.position = _healthBarCanvasPoint.position;

        _healthBar.GetComponent<HealthBarCanvas>().InitSliderValues(value, maxValue);
        _healthBar.GetComponent<HealthBarCanvas>().FollowBy(_healthBarCanvasPoint);
    }

    public void ApplyDamage(float damageValue)
    {
        _currentHealth -= damageValue;

        _healthBar.GetComponent<HealthBarCanvas>().SetSliderValues(_currentHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
}
