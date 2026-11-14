using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("=====CONSTANTS=====")]

    [Header("Moving Values")]
    public float walk_speed;
    public float sprint_speed;
    public float turningSpeed = 100f;

    [Header("Vertical Axis Management")]
    public float jump_strength = 8.0f;
    public float gravity = 1f;

    [Header("Double Jump")]
    public bool double_jump_active;
    public float max_double_jump_cooldown = 0.5f;

    [Header("=====VARIABLES=====")]

    [Header("Movement")]
    public float current_speed;
    public Vector3 move;
    public Quaternion goalRotation;

    [Header("Jumping")]
    public bool has_double_jumped;
    public bool is_jumping;
    public float double_jump_cooldown;

    [Header("=====COMPONENTS=====")]
    public PlayerInputHandler player_input_handler;
    public Animator animator;
    public CharacterController character_controller;

    [Header("=======OTHER=======")]
    public Transform camera;

    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player_input_handler = GetComponent<PlayerInputHandler>();
        character_controller = GetComponent<CharacterController>();
    }

    void Update ()
    {
        HandleMovement();
    }

    private void HandleMovement ()
    {
        if (player_input_handler.SprintTriggered) { current_speed = sprint_speed; }
        if (player_input_handler.SprintTriggered != true) { current_speed = walk_speed; }


        Vector3 worldDirection = CalculateWorldDirection();
        move.x = worldDirection.x * current_speed;
        move.z = worldDirection.z * current_speed;

        if (move.x != 0 || move.z != 0)
        {
            Quaternion aRotation = transform.rotation;
            transform.eulerAngles = new Vector3(0f, camera.eulerAngles.y + (Mathf.Atan2(player_input_handler.MovementInput.y, player_input_handler.MovementInput.x) * Mathf.Rad2Deg), 0f);
            goalRotation = transform.rotation;
            transform.rotation = aRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, turningSpeed * Time.deltaTime);
        }

        HandleVerticalMovement();
        character_controller.Move(move * Time.deltaTime);
    }

    private void HandleVerticalMovement ()
    {
        if (character_controller.isGrounded)
        {
            move.y = -0.5f;
            is_jumping = false;
            has_double_jumped = false;
            double_jump_cooldown = max_double_jump_cooldown;
            if (player_input_handler.JumpTriggered)
            {
                move.y = jump_strength;
                is_jumping = true;
            }
        }
        else
        {
            move.y += Physics.gravity.y * gravity * Time.deltaTime;
            if (is_jumping == false)
            {
                is_jumping = true;
                double_jump_cooldown = 0.02f;      
            }
        }
        if (is_jumping == true)
        {
            double_jump_cooldown -= Time.deltaTime;
            if (double_jump_cooldown < 0 && player_input_handler.JumpTriggered && has_double_jumped == false && double_jump_active)
            {
                move.y = jump_strength;
                has_double_jumped = true;
            }
        }
        
    }

    private Vector3 CalculateWorldDirection ()
    {
        Vector3 inputDirection = new Vector3(player_input_handler.MovementInput.x, 0f, player_input_handler.MovementInput.y);
        Vector3 worldDirection = camera.TransformDirection(inputDirection);
        return worldDirection;
    }
}
