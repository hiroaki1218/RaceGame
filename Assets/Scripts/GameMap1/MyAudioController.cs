using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyAudioController : MonoBehaviour
{
    [SerializeField] AudioSource Normalaudiosource;
    [SerializeField] AudioSource Driftingaudiosource;
    [SerializeField] AudioSource Specialaudiosource;
    [SerializeField] AudioClip BoostingClip;

    [Header("OtherScript")]
    [SerializeField] InputManager inputmanager;
    [SerializeField] ItemGetAndSet itemgetandset;

    public bool isGambler;
    private bool isnormal;
    private bool isdrifting;
    private bool isboosting;
    private bool isSpecial;

    private void Start()
    {
        isnormal = false;
        isdrifting = false;
        isboosting = false;
        isSpecial = false;
    }

    void FixedUpdate()
    {
        if (!GameMap1Controller.isStartedMatch && SceneManager.GetActiveScene().name == "GameMap1")
        {
            return;
        }
        else if (itemgetandset.isSpecial && isGambler)
        {
            if (!isSpecial)
            {
                Specialaudiosource.Play();
                isSpecial = true;
            }

            if (!Specialaudiosource.isPlaying)
            {
                itemgetandset.isSpecial = false;
            }

            //Normal
            isnormal = false;
            Normalaudiosource.Stop();

            //Drifting
            isdrifting = false;
            Driftingaudiosource.Stop();

            //Boosting
            isboosting = false;
        }
        else if (inputmanager.boosting)
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

            //special
            Specialaudiosource.Stop();
            isSpecial = false;
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

            //special
            Specialaudiosource.Stop();
            isSpecial = false;
        }
        else if (inputmanager.isMoving)
        {
            if (!isnormal)
            {
                Normalaudiosource.Play();
                isnormal = true;
            }
            Normalaudiosource.volume = 1f;

            //Drifting
            isdrifting = false;
            Driftingaudiosource.Stop();

            //Boosting
            isboosting = false;

            //special
            Specialaudiosource.Stop();
            isSpecial = false;
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

            //special
            Specialaudiosource.Stop();
            isSpecial = false;
        }
    }
}
