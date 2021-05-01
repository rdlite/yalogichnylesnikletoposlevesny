using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _playerAnimator;
    private Transform _transform;

    [Range(5f, 20f)]
    [SerializeField] private float _lookAtInputRotationSpeed = 10f;

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _transform = transform;
    }

    public void LookAtInput(Vector2 InputDirection)
    {
        Vector3 relativePos = new Vector3(InputDirection.x, 0f, InputDirection.y);
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, toRotation, _lookAtInputRotationSpeed * Time.deltaTime);
    }

    public void LookAtNearestEnemy()
    {
        if (EnemyGlobalListener.Instance.NearestEnemy != null)
        {
            Vector3 relativePos = EnemyGlobalListener.Instance.NearestEnemy.transform.position - _transform.position;
            relativePos = new Vector3(relativePos.x, 0f, relativePos.z);
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            _transform.rotation = Quaternion.Lerp(_transform.rotation, toRotation, _lookAtInputRotationSpeed * Time.deltaTime);
        }
    }

    public void SetWalkingAnimation(bool isMoving)
    {
        _playerAnimator.SetBool("IsMoving", isMoving);
    }

    public void SetAttack()
    {
        _playerAnimator.SetTrigger("Attack");
    }
}
