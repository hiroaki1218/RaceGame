using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioController : MonoBehaviour
{
    [SerializeField] AudioSource Normalaudiosource;
    [SerializeField] AudioSource Driftingaudiosource;
    [SerializeField] AudioClip BoostingClip;

    [Header("OtherScript")]
    [SerializeField] InputManager inputmanager;

    private bool isnormal;
    private bool isdrifting;
    private bool isboosting;

    private void Start()
    {
        isnormal = false;
        isdrifting = false;
        isboosting = false; 
    }

    void Update()
    {
        if (inputmanager.boosting)
        {
            if (!isboosting)
            {
                Normalaudiosource.PlayOneShot(BoostingClip);
                isboosting = true;
            }

            //Normal
            Normalaudiosource.volume = 0.6f;

            //Drifting
            isdrifting = false;
            Driftingaudiosource.Stop();
        }
        else if (inputmanager.handbrake)
        {
            if (!isdrifting)
            {
                Driftingaudiosource.Play();
                isdrifting = true;
            }

            //Normal
            Normalaudiosource.volume = 0.6f;

            //Boosting
            isboosting = false;
        }
        else if (inputmanager.isMoving)
        {
            if (!isnormal)
            {
                Normalaudiosource.volume = 1f;
                Normalaudiosource.Play();
                isnormal = true;
            }

            //Drifting
            isdrifting = false;
            Driftingaudiosource.Stop();

            //Boosting
            isboosting = false;
        }
        else
        {
            //Normal
            isnormal = false;
            Normalaudiosource.volume = 1f;
            Normalaudiosource.Stop();

            //Drifting
            isdrifting = false;
            Driftingaudiosource.Stop();

            //Boosting
            isboosting = false;
        }
    }
}
