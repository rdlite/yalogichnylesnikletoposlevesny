using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Arrow : MonoBehaviour
{
    [Range(0f, 20f)]
    public float FlyingSpeed;

    [Range(0f, 40f)]
    public float PushPower = 20f;

    [SerializeField] private protected GameObject HitEffect;

    protected Vector3 _target;
    protected float _attackDamage;

    public abstract void InitArrow(Vector3 target, float damage);
    public abstract void InitArrow(Transform target, float damage);

    protected abstract void ArrowFlying();
}
