using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultArrow : Arrow
{
    private Rigidbody _arrowRigidbody;

    public override void InitArrow(Vector3 target, float damage)
    {
        _target = target;
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

            _arrowRigidbody.velocity = (_target - transform.position).normalized * FlyingSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamagable>()?.ApplyDamage(_attackDamage);
        other.GetComponent<IPushable>()?.PushAgainst(transform.position, PushPower);

        Destroy(Instantiate(HitEffect, transform.position, Quaternion.identity), .5f);
        Destroy(gameObject);
    }
}
