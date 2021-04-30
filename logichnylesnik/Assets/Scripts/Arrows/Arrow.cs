using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Arrow : MonoBehaviour
{
    [Range(0f, 10f)]
    public float FlyingSpeed;

    protected Vector3 _target;

    public abstract void InitArrow(Vector3 target);
 
    protected abstract void ArrowFlying();
}
