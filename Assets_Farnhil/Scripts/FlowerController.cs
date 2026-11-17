using UnityEngine;

public class FlowerController : MonoBehaviour
{
    public bool isOpen;
    public Animator animator;
    public bool lastOpenState;
    public GameObject colliders;

    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    void Update ()
    {
        if (isOpen && lastOpenState == false)
        {
            colliders.SetActive(true);
            animator.SetTrigger("bloom");
        }
        lastOpenState = isOpen;
    }
}
