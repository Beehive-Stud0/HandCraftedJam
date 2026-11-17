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
    [SerializeField] private string whistle = "Whistle";
    [SerializeField] private string bark = "Bark/Hint";
    [SerializeField] private string reset = "Reset";

    private InputAction movementAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private InputAction actionAction;
    private InputAction whistleAction;
    private InputAction barkAction;
    private InputAction resetAction;

    public Vector2 MovementInput;
    public bool SprintTriggered;
    public bool JumpTriggered;

    public bool ActionTriggered;
    public bool WhistleTriggered;
    public bool BarkTriggered;
    public bool ResetTriggered;

    private void Awake ()
    {
        InputActionMap actionMapReference = player_controls.FindActionMap(action_map_name);

        movementAction = actionMapReference.FindAction(movement);
        sprintAction = actionMapReference.FindAction(sprint);
        jumpAction = actionMapReference.FindAction(jump);

        actionAction = actionMapReference.FindAction(action);
        whistleAction = actionMapReference.FindAction(whistle);
        barkAction = actionMapReference.FindAction(bark);
        resetAction = actionMapReference.FindAction(reset);
        
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

        whistleAction.performed += inputInfo => WhistleTriggered = true;
        whistleAction.canceled += inputInfo => WhistleTriggered = false;

        barkAction.performed += inputInfo => BarkTriggered = true;
        barkAction.canceled += inputInfo => BarkTriggered = false;
        
        resetAction.performed += inputInfo => ResetTriggered = true;
        resetAction.canceled += inputInfo => ResetTriggered = false;
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
