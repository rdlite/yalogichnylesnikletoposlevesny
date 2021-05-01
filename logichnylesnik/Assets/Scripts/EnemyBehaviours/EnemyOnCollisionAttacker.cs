using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnCollisionAttacker : MonoBehaviour
{
    private float _attackDamage;

    public void InitEnemyAttacker(float damageValue)
    {
        _attackDamage = damageValue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null)
        {
            collision.gameObject.GetComponent<IDamagable>()?.ApplyDamage(_attackDamage);
        }
    }
}
