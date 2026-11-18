using UnityEngine;
using System.Collections.Generic;

public class LadybugController : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public bool isPlayerRiding;
    public int point_index;
    public float speed = 15f;
    public GameObject player;
    private Quaternion lookRotation;
    private Quaternion current_rotation;
    public Animator animator;
    public GameManager gm;

    void Start ()
    {
        gm = GameObject.FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            gm.ladybug = true;
            isPlayerRiding = true;
            player = other.gameObject;
            player.transform.parent = transform;
            player.GetComponent<PlayerController>().canMove = false;
        }
    }

    void Update ()
    {
        if (isPlayerRiding)
        {
            animator.SetBool("isFlying", true);
            player.GetComponent<PlayerController>().canMove = false;

            Vector3 lookPos = transform.position - points[point_index].position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            transform.position = Vector3.MoveTowards(transform.position, points[point_index].position, speed * Time.deltaTime);

            if (transform.position == points[point_index].position)
            {
                if (point_index == points.Count-1)
                {
                    isPlayerRiding = false;
                    player.transform.parent = null;
                    player.GetComponent<PlayerController>().canMove = true;
                    point_index = 0;
                    points.Reverse();
                }
                else
                {
                    point_index += 1;
                }
            }
        }
        else {
            animator.SetBool("isFlying", false);
        }
    }
}
