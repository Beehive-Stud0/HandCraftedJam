using UnityEngine;

public class Objective : MonoBehaviour
{
    public bool canGive;
    public int id;
    public GameManager gm;
    public bool hasToActivateParticles;
    public ParticleSystem particles;
    public bool secondCondition = true;

    void Start ()
    {
        gm = FindAnyObjectByType<GameManager>();
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
