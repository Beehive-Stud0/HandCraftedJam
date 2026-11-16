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

    [SerializeField] private string action = "Action";

    private InputAction movementAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private InputAction actionAction;

    public Vector2 MovementInput;
    public bool SprintTriggered;
    public bool JumpTriggered;

    public bool ActionTriggered;

    private void Awake ()
    {
        InputActionMap actionMapReference = player_controls.FindActionMap(action_map_name);

        movementAction = actionMapReference.FindAction(movement);
        sprintAction = actionMapReference.FindAction(sprint);
        jumpAction = actionMapReference.FindAction(jump);

        actionAction = actionMapReference.FindAction(action);
        
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

        actionAction.performed += inputInfo => ActionTriggered = true;
        actionAction.canceled += inputInfo => ActionTriggered = false;
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
