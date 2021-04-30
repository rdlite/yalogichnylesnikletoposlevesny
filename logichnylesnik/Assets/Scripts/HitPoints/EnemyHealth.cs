using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    private float _currentHealth = 100f;
    //[SerializeField] private 

    public void InitHeath(float value)
    {
        _currentHealth = value;
    }

    public void ApplyDamage(float damageValue)
    {

    }
}
