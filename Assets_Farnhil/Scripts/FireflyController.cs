using UnityEngine;
using System.Collections;

public class FireflyController : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public bool isHiding;
    public float speed;
    public Vector3 lastPosition;
    public GameObject[] flowers;
    public bool hasOpenedFlowers;
    
    void Start ()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        lastPosition = transform.position;

        flowers = GameObject.FindGameObjectsWithTag("Flower");
    }

    void Update ()
    {
        if (isHiding == false && hasOpenedFlowers == false)
        {
            animator.SetBool("isHiding", false);
            if (Vector3.Distance(transform.position, player.transform.position) > 8 )
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 10 * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, player.transform.position.y + 2, transform.position.z);
            }
            speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            animator.SetFloat("speed", speed);
            Vector3 lookPos = transform.position - player.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 50);
        }
        if (Vector3.Distance(transform.position, flowers[0].transform.position) < 10)
        {
            StartCoroutine(OpenFlowers());
            hasOpenedFlowers = true;
        }
        if (hasOpenedFlowers)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 50, 10), 15 * Time.deltaTime);
        }
        lastPosition = transform.position;
    }

    IEnumerator OpenFlowers ()
    {
        foreach (GameObject flower in flowers)
        {
            flower.GetComponent<FlowerController>().isOpen = true;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);

    }
}
