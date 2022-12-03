using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] SmokeEffect;
    [SerializeField] InputManager inputmanager;
    [SerializeField] Color normalColor;
    [SerializeField] Color movingColor;
    [SerializeField] Color boostingColor;
    [SerializeField] Color handbrake1Color;
    [SerializeField] Color handbrake2Color;

    private void Start()
    {
        for(int i=0; i< SmokeEffect.Length; i++)
        {
            SmokeEffect[i].Play();
            ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
            em.rateOverTime = new ParticleSystem.MinMaxCurve(30);
        }   
    }

    private void FixedUpdate()
    {
        if (inputmanager.boosting || inputmanager.handbrake)
        {
            BoostAndHandBrakeSmokeAnime();
        }
        else
        {
            NormalSmokeAnime();
        }  
    }

    private void NormalSmokeAnime()
    {
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;

        //車が動いていたらパーティクル色チェン
        if (inputmanager.isMoving)
        {
            for (int i = 0; i < SmokeEffect.Length; i++)
            {
                color.color = movingColor;
                ParticleSystem.MainModule main = SmokeEffect[i].main;
                main.startColor = color;

                ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                em.rateOverTime = new ParticleSystem.MinMaxCurve(30);
            }
        }
        else
        {
            for (int i = 0; i < SmokeEffect.Length; i++)
            {
                color.color = normalColor;
                ParticleSystem.MainModule main = SmokeEffect[i].main;
                main.startColor = color;

                ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                em.rateOverTime = new ParticleSystem.MinMaxCurve(30);
            }
        }
    }

    private void BoostAndHandBrakeSmokeAnime()
    {
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;

        if (inputmanager.boosting)
        {
            for (int i = 0; i < SmokeEffect.Length; i++)
            {
                color.color = boostingColor;
                ParticleSystem.MainModule main = SmokeEffect[i].main;
                main.startColor = color;

                ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                em.rateOverTime = new ParticleSystem.MinMaxCurve(100);
            }
        }
        else
        {
            if (BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.6)
            {
                for (int i = 0; i < SmokeEffect.Length; i++)
                {
                    color.color = boostingColor;
                    ParticleSystem.MainModule main = SmokeEffect[i].main;
                    main.startColor = color;

                    ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                    em.rateOverTime = new ParticleSystem.MinMaxCurve(80);
                }
            }
            else if (BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.3)
            {
                for (int i = 0; i < SmokeEffect.Length; i++)
                {
                    color.color = handbrake2Color;
                    ParticleSystem.MainModule main = SmokeEffect[i].main;
                    main.startColor = color;

                    ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                    em.rateOverTime = new ParticleSystem.MinMaxCurve(60);
                }
            }
            else if(BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.15)
            {
                for (int i = 0; i < SmokeEffect.Length; i++)
                {
                    color.color = handbrake1Color;
                    ParticleSystem.MainModule main = SmokeEffect[i].main;
                    main.startColor = color;

                    ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                    em.rateOverTime = new ParticleSystem.MinMaxCurve(40);
                }
            }
            else
            {
                for (int i = 0; i < SmokeEffect.Length; i++)
                {
                    color.color = movingColor;
                    ParticleSystem.MainModule main = SmokeEffect[i].main;
                    main.startColor = color;

                    ParticleSystem.EmissionModule em = SmokeEffect[i].emission;
                    em.rateOverTime = new ParticleSystem.MinMaxCurve(35);
                }
            }
        }
    }

    private void HandBrakeAnime()
    {
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;

        
    }
}
