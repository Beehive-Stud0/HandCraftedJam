using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NewDogBehaviour : MonoBehaviour
{
    [Header("Player Stuff")]
    public GameObject player;
    public Transform playerTransform;
    public float minDistToPlayer;

    [Header("Actions Related")]
    public GameObject[] objectives;
    public Transform closestObjective;
    public bool is_sitting;
    public bool isBarking;
    // public float sitting_toggle_cooldown;

    [Header("Movement")]
    public float moving_speed;
    public NavMeshAgent agent;
    public Vector3 lastPosition;

    [Header("Animator")]
    public Animator animator;

    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(playerTransform.position);
        lastPosition = transform.position;

        objectives = GameObject.FindGameObjectsWithTag("Objective");
    }

    void Update ()
    {
        moving_speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        if (is_sitting == false && isBarking == false)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);

            animator.SetBool("isSitting", false);
            animator.SetFloat("speed", moving_speed);
        }
        else
        {
            agent.isStopped = true;
            if (is_sitting)
            {
                animator.SetBool("isSitting", true);
            }
            if (isBarking)
            {
                animator.SetBool("isBarking", true);
            }
        }
        lastPosition = transform.position;
    }

    public void Bark ()
    {
        objectives = GameObject.FindGameObjectsWithTag("Objective");
        is_sitting = false;
        closestObjective = objectives[0].transform;
        foreach (GameObject current_go in objectives)
        {
            if (Vector3.Distance(transform.position, current_go.transform.position) < Vector3.Distance(transform.position, closestObjective.position))
            {
                closestObjective = current_go.transform;
            }
        }
        if (isBarking == true)
        {
            return;
        }
        if (isBarking == false)
        {
            isBarking = true;
            StartCoroutine(BarkAnimation());
        }
    }

    IEnumerator BarkAnimation()
    {
        var lookPos = closestObjective.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        for (int i = 0 ; i < 20 ; i++)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 35 * Time.deltaTime);
            yield return new WaitForSeconds(0.03f);
        }
        animator.SetBool("isBarking", true);
        yield return new WaitForSeconds((float) Random.Range(7, 25) / 10f);
        animator.SetBool("isBarking", false);
        isBarking = false;
    }

    public void ToggleSitting ()
    {
        // if (sitting_toggle_cooldown < 0)
        // {
            // sitting_toggle_cooldown = 0.8f;
            if (is_sitting ==  true)
            {
                is_sitting = false;
                return;
            }
            if (is_sitting == false)
            {
                is_sitting = true;
                return;
            }
        // }
    }
}
