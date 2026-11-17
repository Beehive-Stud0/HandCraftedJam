using UnityEngine;

public class DogBehaviour : MonoBehaviour
{
    [Header("Player Stuff")]
    public GameObject player;
    public Transform playerTransform;
    public Vector3 teleport_player_offset;

    [Header("Actions Related")]
    public GameObject[] objectives;
    public Transform closestObjective;
    public bool is_sitting;

    [Header("Movement")]
    public float moving_speed;
    public CharacterController character_controller;
    public float jump_strength;
    public Vector3 move;

    [Header("Back to Player")]
    public Vector3 lastPosition;
    public float current_speed;
    public float low_speed_time;
    public float max_low_speed_time;

    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        character_controller = GetComponent<CharacterController>();
        character_controller.enabled = true;

        objectives = GameObject.FindGameObjectsWithTag("Objective");
    }

    void Update ()
    {
        current_speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        
        HandleMovement();
        lastPosition = transform.position;
    }

    void HandleMovement ()
    {
        move = new Vector3();
        if (Vector3.Distance(transform.position, playerTransform.position) > 4 || character_controller.isGrounded != true)
        {
            Vector3 targetPosition = playerTransform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            move.x = transform.TransformDirection(Vector3.forward).x * moving_speed;
            move.z = transform.TransformDirection(Vector3.forward).z * moving_speed;
        }
        HandleVerticalMovement();
        character_controller.Move(move * Time.deltaTime);
    }

    void HandleVerticalMovement ()
    {
        if (character_controller.isGrounded)
        {
            float yOffset = playerTransform.position.y - transform.position.y;
            bool has_to_jump = yOffset > 0.6f && Vector3.Distance(transform.position, playerTransform.position) < yOffset * 2.5f;
            if (has_to_jump)
            {
                move.y = jump_strength;
            }
            else
            {
                move.y = -0.5f;
            }
        }
        else
        {
            move.y += Physics.gravity.y * Time.deltaTime * 1f;
        }
    }

    public void Bark ()
    {
        is_sitting = false;
        closestObjective = objectives[0].transform;
        foreach (GameObject current_go in objectives)
        {
            if (Vector3.Distance(transform.position, current_go.transform.position) < Vector3.Distance(transform.position, closestObjective.position))
            {
                closestObjective = current_go.transform;
            }
        }
    }

    public void ToggleSitting ()
    {
        if (is_sitting)
        {
            is_sitting = false;
            return;
        }
        if (is_sitting != true)
        {
            is_sitting = true;
            return;
        }
    }

    public void TeleportToPlayer ()
    {
        transform.position = playerTransform.position + teleport_player_offset;
    }
}
