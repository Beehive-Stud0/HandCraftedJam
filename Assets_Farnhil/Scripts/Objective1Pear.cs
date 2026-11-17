using UnityEngine;

public class Objective1Pear : MonoBehaviour
{
    public bool isPear;
    public Objective objective;

    void Start ()
    {
        objective.secondCondition = false;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "pushable")
        {
            isPear = true;
            objective.secondCondition = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag == "pushable")
        {
            isPear = false;
            objective.secondCondition = false;
        }
    }
}
