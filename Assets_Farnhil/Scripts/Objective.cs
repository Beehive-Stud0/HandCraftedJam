using UnityEngine;

public class Objective : MonoBehaviour
{
    public bool canGive;
    public int id;
    public GameManager gm;
    public bool hasToActivateParticles;
    public ParticleSystem particles;
    public bool secondCondition = true;

    public bool isBottleHere;

    void Start ()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Bottle")
        {
            isBottleHere = true;
        }
    }
    void OnTriggerExit (Collider other)
    {
        if (other.tag == "Bottle")
        {
            isBottleHere = false;
        }
    }

    void OnTriggerStay (Collider other)
    {
        if (other.tag == "Bottle" && canGive && gm.ingredients.Contains(id) == false && secondCondition)
        {
            if (hasToActivateParticles) { particles.Play(); }
            gm.ingredients.Add(id);
        }
    }
}
