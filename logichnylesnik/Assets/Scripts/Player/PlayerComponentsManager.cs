using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAttacker))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerComponentsManager : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;

    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    private PlayerAttacker _playerAttacker;
    private PlayerAnimation _playerAnimation;

    private void Start()
    {
        EnemyGlobalListener.Instance.InitPlayer(transform);

        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        _playerAttacker = GetComponent<PlayerAttacker>();
        _playerAttacker.SetAttackSpeed(_playerStats.GetAttackSpeed());

        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        _playerInput.SetInput();

        _playerInput.SetInputDirectionIndicatorPosition();

        if (_playerInput.IsMoving())
        {
            _playerAnimation.LookAtInput(_playerInput.GetInput());
        }
        else
        {
            _playerAnimation.LookAtNearestEnemy();
            _playerAttacker.AttackingEvent();
        }
    }

    private void FixedUpdate()
    {
        _playerMovement.MovePlayer(_playerInput.GetInput(), _playerStats.GetMoveSpeed());
    }
}
