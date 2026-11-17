using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    public Objective triggerObjective;
    public bool isPressed;
    public float yDown;
    public Vector3 basePosition;

    void Start ()
    {
        basePosition = transform.position;
    }

    void OnTriggerEnter (Collider other)
    {
        Debug.Log("press");
        if (other.tag == "Dog")
        {
            isPressed = true;
            triggerObjective.canGive = true;
            StartCoroutine(GetDown());
        }
    }

    void OnTriggerExit (Collider other)
    {
        isPressed = false;
        triggerObjective.canGive = false;
        StartCoroutine(GetUp());
    }

    IEnumerator GetDown ()
    {
        for (float i = 10; i > 0; i -= 1)
        {
            transform.position = new Vector3(basePosition.x, basePosition.y + (yDown * (i/10)), basePosition.z);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator GetUp ()
    {
        for (float i = 0; i < 10; i++)
        {
            transform.position = new Vector3(basePosition.x, basePosition.y + (-yDown * (i/10)), basePosition.z);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
