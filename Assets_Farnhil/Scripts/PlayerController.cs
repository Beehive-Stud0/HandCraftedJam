using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("/======/ MOVEMENT /======/")]
    [Header("")]

    [Header("=====CONSTANTS=====")]

    [Header("Moving Values")]
    public float walk_speed;
    public float sprint_speed;
    public float turningSpeed = 100f;
    public float pushStrength = 50f;

    [Header("Vertical Axis Management")]
    public float jump_strength = 8.0f;
    public float gravity = 1f;

    [Header("Double Jump")]
    public bool double_jump_active;
    public float max_double_jump_cooldown = 0.5f;

    [Header("=====VARIABLES=====")]

    [Header("Movement")]
    public float current_speed;
    public float current_speed_coefficient = 1f;
    public Vector3 move;
    public Quaternion goalRotation;
    public bool canMove = true;

    [Header("Jumping")]
    public bool has_double_jumped;
    public bool is_jumping;
    public float double_jump_cooldown;

    [Header("/======/ ACTIONS /======/")]
    [Header("")]

    [Header("General Action Settings")]
    public float action_trigger_max_cooldown = 0.3f;
    public float action_trigger_cooldown;
    public bool ActionTriggered;

    [Header("====LIFT BOTTLE====")]

    [Header("Bottle Lifting Settings")]
    public GameObject bottle; // Found automatically on start of the scene
    public float maximal_bottle_distance; // Minimal distance to grab the bottle
    public float maximal_y_bottle_offset; // Minimal distance in vertical axis to grab the bottle
    public float bottle_height_offset; // Height above the player origin for the bottle to be held
    public float bottle_lift_speed_coefficient; // Coefficient to decrease the player's speed when lifting the bottle
    public float bottle_lift_animation_speed;

    [Header("Bottle Lifting Variables")]
    public float bottle_distance;
    public float y_bottle_offset;
    public bool is_bottle_lifted;

    [Header("========DOG========")]

    [Header("Dog Reference")]
    public GameObject dogGameObject;
    public Transform dogTransform;
    public NewDogBehaviour dogBehaviour;

    [Header("Dog Position")]
    public float max_distance_to_come_on_whistle; // If the player gets farer than this value, he whistles and his dog is back at his side
    public bool BarkTriggered;
    public bool WhistleTriggered;
    public bool lastWhistlePressState;
    public Vector3 tp_dog_offset;

    [Header("/======/ GENERAL /======/")]
    [Header("")]

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

        bottle = GameObject.FindWithTag("Bottle");

        dogGameObject = GameObject.FindWithTag("Dog");
        dogTransform = dogGameObject.transform;
        dogBehaviour = dogGameObject.GetComponent<NewDogBehaviour>();
    }

    void Update ()
    {
        if (canMove) { HandleMovement(); }
        HandleObjectInteraction();
        HandleDogActions();
    }

    private void HandleMovement ()
    {
        if (player_input_handler.SprintTriggered) { current_speed = sprint_speed; }
        if (player_input_handler.SprintTriggered != true) { current_speed = walk_speed; }


        Vector3 worldDirection = CalculateWorldDirection();
        move.x = worldDirection.x * current_speed * current_speed_coefficient;
        move.z = worldDirection.z * current_speed * current_speed_coefficient;

        if (move.x != 0 || move.z != 0)
        {
            Quaternion aRotation = transform.rotation;
            transform.eulerAngles = new Vector3(0f, camera.eulerAngles.y + (Mathf.Atan2(player_input_handler.MovementInput.y, player_input_handler.MovementInput.x) * Mathf.Rad2Deg-90f), 0f);
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

    private void HandleObjectInteraction ()
    {
        if (action_trigger_cooldown >= 0)
        {
            action_trigger_cooldown -= Time.deltaTime;
        }
        if (player_input_handler.ActionTriggered != true || action_trigger_cooldown > 0)
        {
            ActionTriggered = false;
        }
        if (action_trigger_cooldown < 0 && player_input_handler.ActionTriggered)
        {
            ActionTriggered = true;
            action_trigger_cooldown = action_trigger_max_cooldown;
        }
        

        HandleBottleInteraction();
    }

    private void HandleBottleInteraction ()
    {
        if (is_bottle_lifted != true)
        {
            bottle_distance = Vector3.Distance(transform.position, bottle.transform.position);
            y_bottle_offset = Mathf.Abs(transform.position.y - bottle.transform.position.y);
            current_speed_coefficient = 1f;
        }
        if (is_bottle_lifted)
        {
            current_speed_coefficient = bottle_lift_speed_coefficient;
            if (ActionTriggered)
            {
                PutBottleDown();
            }
        }
        if (bottle_distance <= maximal_bottle_distance && y_bottle_offset <= maximal_y_bottle_offset && is_bottle_lifted == false)
        {
            if (ActionTriggered)
            {
                LiftBottle();
            }
        }
    }

    private void LiftBottle ()
    {
        Debug.Log("lift");
        is_bottle_lifted = true;
        bottle.GetComponent<Rigidbody>().isKinematic = true;
        transform.LookAt(bottle.transform.position);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        StartCoroutine(TakeBottleUp());
    }

    IEnumerator TakeBottleUp ()
    {
        canMove = false;
        Vector3 goalPosition = new Vector3(transform.position.x, transform.position.y + bottle_height_offset, transform.position.z);
        while (bottle.transform.position != goalPosition)
        {
            bottle.transform.position = Vector3.MoveTowards(bottle.transform.position, goalPosition, bottle_lift_animation_speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime); 
        }
        bottle.transform.parent = transform;
        canMove = true;
        yield return new WaitForSeconds(1f);
    }

    private void PutBottleDown ()
    {
        if (canMove)
        {
            StartCoroutine(PutBottleDownEnumerator());
        }
    }

    IEnumerator PutBottleDownEnumerator ()
    {
        canMove = false;
        Vector3 goalPosition = new Vector3(0, -bottle_height_offset+2f, 4f);
        while (bottle.transform.localPosition != goalPosition)
        {
            bottle.transform.localPosition = Vector3.MoveTowards(bottle.transform.localPosition, goalPosition, bottle_lift_animation_speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime); 
        }
        bottle.transform.parent = null;
        canMove = true;
        bottle.GetComponent<Rigidbody>().isKinematic = false;
        is_bottle_lifted = false;
    }

    private void HandleDogActions ()
    {
        if (player_input_handler.WhistleTriggered)
        {
            if (Vector3.Distance(transform.position, dogTransform.position) > 50)
            {
                dogGameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position + tp_dog_offset);
            }
            else if (lastWhistlePressState == false)
            {
                dogBehaviour.ToggleSitting();
            }
        }
        if (player_input_handler.BarkTriggered)
        {
            dogBehaviour.Bark();
        }
        
        lastWhistlePressState = player_input_handler.WhistleTriggered;
    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && hit.gameObject.tag == "pushable")
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * pushStrength, transform.position, ForceMode.Impulse);
        }
    }
}
