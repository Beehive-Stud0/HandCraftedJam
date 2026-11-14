using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset player_controls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string action_map_name = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string jump = "Jump";

    private InputAction movementAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    public Vector2 MovementInput;
    public bool SprintTriggered;
    public bool JumpTriggered;

    private void Awake ()
    {
        InputActionMap actionMapReference = player_controls.FindActionMap(action_map_name);

        movementAction = actionMapReference.FindAction(movement);
        sprintAction = actionMapReference.FindAction(sprint);
        jumpAction = actionMapReference.FindAction(jump);
        
        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents ()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;
    }

    private void OnEnable ()
    {
        player_controls.FindActionMap(action_map_name).Enable();
    }

    private void OnDisable ()
    {
        player_controls.FindActionMap(action_map_name).Disable();
    }
}
