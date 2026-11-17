using UnityEngine;
using Unity.Cinemachine;

public class rockCameraRadiusChange : MonoBehaviour
{
    public bool isPlayerHere;
    public CinemachineOrbitalFollow cameraFollow;
    public bool isZoomingIn;
    public bool isZoomingOut;

    public float maxRadius = 11f;
    public float minRadius;

    public float transitionTime;
    public float maxTransitionTime = 3f;

    void Start ()
    {
        cameraFollow = GameObject.FindAnyObjectByType<CinemachineOrbitalFollow>();
        minRadius = cameraFollow.Radius;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            isZoomingIn = true;
            isZoomingOut = false;
            transitionTime = 0f;
        }
    }
    void OnTriggerExit (Collider other)
    {
        if (other.tag == "Player")
        {
            isZoomingOut = true;
            isZoomingIn = false;
            transitionTime = maxTransitionTime;
        }
    }

    void Update ()
    {
        if (isZoomingIn)
        {
            transitionTime += Time.deltaTime;
            cameraFollow.Radius = Mathf.Lerp(minRadius, maxRadius, transitionTime / maxTransitionTime);
            if (transitionTime > 3)
            {
                isZoomingIn = false;
            }
        }
        if (isZoomingOut)
        {
            transitionTime -= Time.deltaTime;
            cameraFollow.Radius = Mathf.Lerp(minRadius, maxRadius, transitionTime / maxTransitionTime);
            if (transitionTime < 0)
            {
                isZoomingOut = false;
            }
        }
    }
}
