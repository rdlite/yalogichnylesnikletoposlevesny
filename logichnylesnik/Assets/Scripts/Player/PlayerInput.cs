using CnControls;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 JoystickMovementDirection;

    [SerializeField] private string _joystickHorizontalInput, _joystickVerticalInput;

    [SerializeField] private GameObject InputDirectionIndicator;
    [SerializeField] private float InputDirectionIndicatorSpreadSize;

    public bool IsMoving()
    {
        return GetInput() != Vector2.zero;
    }

    public Vector2 GetInput()
    {
        return JoystickMovementDirection;
    }

    public void SetInput()
    {
        JoystickMovementDirection = new Vector2(CnInputManager.GetAxis(_joystickHorizontalInput), CnInputManager.GetAxis(_joystickVerticalInput));
    }

    public void SetInputDirectionIndicatorPosition()
    {
        InputDirectionIndicator.SetActive(IsMoving());

        if (IsMoving())
        {
            InputDirectionIndicator.transform.position = 
                new Vector3(
                    transform.position.x, 
                    InputDirectionIndicator.transform.position.y, 
                    transform.position.z
                    ) 
                + 
                    new Vector3(
                        GetInput().x, 
                        0f, 
                        GetInput().y
                        ) 
                * InputDirectionIndicatorSpreadSize;
        }
    }
}
