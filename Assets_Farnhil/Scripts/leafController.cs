using UnityEngine;

public class leafController : MonoBehaviour
{
    public Objective objective3;
    public bool canFall;
    public Animator animator;
    
    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "pushable")
        {
            canFall = true;
        }
    }

    void Update ()
    {
        if (objective3.isBottleHere && canFall)
        {
            objective3.canGive = true;
            Debug.Log("ploc");
        }
    }
}
