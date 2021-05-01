using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAttacker))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerComponentsManager : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;

    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    private PlayerAttacker _playerAttacker;
    private PlayerAnimation _playerAnimation;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        EnemyGlobalListener.Instance.InitPlayer(transform);

        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        _playerAttacker = GetComponent<PlayerAttacker>();
        _playerAttacker.SetAttackDamage(_playerStats.GetAttackDamage());
        _playerAttacker.SetAttackSpeed(_playerStats.GetAttackSpeed());

        _playerAnimation = GetComponent<PlayerAnimation>();

        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.InitHeath(_playerStats.GetMaxHealth());

        _playerAttacker.OnShoot += _playerAnimation.SetAttack;
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
     
        _playerAnimation.SetWalkingAnimation(_playerInput.IsMoving());
    }

    private void FixedUpdate()
    {
        _playerMovement.MovePlayer(_playerInput.GetInput(), _playerStats.GetMoveSpeed());
    }
}
