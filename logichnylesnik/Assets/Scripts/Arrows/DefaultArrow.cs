using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultArrow : Arrow
{
    private Rigidbody _arrowRigidbody;

    private Vector3 _direction;

    public override void InitArrow(Vector3 target, float damage)
    {
        _target = target;
        _direction = (_target - transform.position).normalized;
        _attackDamage = damage;

        _arrowRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(FixedCoroutineUpdate());
    }

    private IEnumerator FixedCoroutineUpdate()
    {
        while (gameObject.activeSelf)
        {
            ArrowFlying();

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void ArrowFlying()
    {
        if (_target != null)
        {
            transform.LookAt(_target);

            _arrowRigidbody.velocity = _direction * FlyingSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamagable>()?.ApplyDamage(_attackDamage);
        other.GetComponent<IPushable>()?.PushAgainst(transform.forward, PushPower);

        Destroy(Instantiate(HitEffect, transform.position, Quaternion.identity), .5f);
        Destroy(gameObject);
    }
}
