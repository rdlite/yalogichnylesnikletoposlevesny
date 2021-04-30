using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MovePlayer(Vector2 Direction, float MoveSpeed)
    {
        _rigidbody.velocity = new Vector3(Direction.x, 0, Direction.y) * MoveSpeed;
    }
}