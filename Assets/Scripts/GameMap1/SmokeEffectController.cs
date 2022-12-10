using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SmokeEffectController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject normalSmokeEffect;
    [SerializeField] GameObject driftingSmokeEffect;
    [SerializeField] GameObject boostEffect;
    [SerializeField] Transform EffectPosition;
    [SerializeField] InputManager inputmanager;
    [SerializeField] Color normalColor;
    [SerializeField] Color movingColor;
    [SerializeField] Color boostingColor;
    [SerializeField] Color handbrake1Color;
    [SerializeField] Color handbrake2Color;

    public PhotonView myPV;
    public bool once;

    private void Start()
    {
        myPV = GetComponent<PhotonView>();
        once = true;
        normalSmokeEffect.SetActive(true);
        driftingSmokeEffect.SetActive(false);
        boostEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (myPV.IsMine)
        {
            mySmokeChange();
        } 
    }

    
    private void mySmokeChange()
    {
        if (inputmanager.boosting)
        {
            myPV.RPC(nameof(BoostSmokeAnime), RpcTarget.All);
        }
        else if(inputmanager.handbrake || inputmanager.isMoving)
        {
            myPV.RPC(nameof(MoveSmokeAnime), RpcTarget.All);
        }
        else
        {
            myPV.RPC(nameof(NormalSmokeAnime), RpcTarget.All);
        }
    }

    [PunRPC]
    private void NormalSmokeAnime()
    {
        normalSmokeEffect.SetActive(true);
        driftingSmokeEffect.SetActive(false);
    }

    [PunRPC]
    private void MoveSmokeAnime()
    {
        //車が動いていたらパーティクル色チェン
        driftingSmokeEffect.SetActive(true);
        normalSmokeEffect.SetActive(false);
    }

    private void HandBrakeSmokeAnime()
    {

        if (BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.6)
        {
            
        }
        else if (BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.3)
        {
            
        }
        else if (BoostController.instance.driftTime / BoostController.instance.maxdrifttime >= 0.15)
        {
            
        }
        else
        {
            
        }
    }

    [PunRPC]
    private void BoostSmokeAnime()
    {
        if (once)
        {
            boostEffect.SetActive(true);
            driftingSmokeEffect.SetActive(false);
            normalSmokeEffect.SetActive(false);
            StartCoroutine(Waiting());
            once = false;
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        boostEffect.SetActive(false);
        once = true;
    }
}
