using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public List<int> ingredients = new List<int>();
    public AudioSource musicSource;
    public PlayerController playerControl;
    public AudioSource objectiveSound;

    public AudioClip intro;
    public AudioClip benchToLadybug;
    public AudioClip ladyBugToDirectview;
    public AudioClip fromDirectwiew;
    public AudioClip ending;

    public bool hasBegin;
    public bool ladybug;
    public bool directView;
    public bool finish;

    public bool lastBeginState;

    public UIDocument uidoc;

    void Start ()
    {
        playerControl = GameObject.FindAnyObjectByType<PlayerController>();
        playerControl.canMove = false;
        uidoc = GetComponent<UIDocument>();
        StartCoroutine(waitForUIDocumentToFinishPlaying());
    }

    void Update ()
    {
        if (lastBeginState == false && hasBegin)
        {
            playerControl.canMove = true;
        }
        if (hasBegin == false && musicSource.clip != intro)
        {
            musicSource.Stop();
            musicSource.clip = intro;
            musicSource.Play();
            
        }
        if (hasBegin)
        {
            if (ladybug == false && directView == false && finish == false && musicSource.clip != benchToLadybug)
            {
                musicSource.Stop();
                musicSource.clip = benchToLadybug;
                musicSource.Play();
            }
            if (ladybug && directView == false && finish == false && musicSource.clip != ladyBugToDirectview)
            {
                musicSource.Stop();
                musicSource.clip = ladyBugToDirectview;
                musicSource.Play();
            }
            if (ladybug && directView && finish == false && musicSource.clip != fromDirectwiew)
            {
                musicSource.Stop();
                musicSource.clip = fromDirectwiew;
                musicSource.Play();
            }
            if (ladybug && directView && finish && musicSource.clip != ending)
            {
                musicSource.Stop();
                musicSource.clip = ending;
                musicSource.Play();
            }
        }
        lastBeginState = hasBegin;
    }

    IEnumerator waitForUIDocumentToFinishPlaying ()
    {
        yield return new WaitForSeconds(20f);
        uidoc.enabled = false;
    }

    public void PlayObjectiveSound()
    {
        objectiveSound.Play();
    }
    
}
