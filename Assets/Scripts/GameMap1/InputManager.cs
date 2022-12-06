using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public BoostController boostController;
    public bool isMoving;
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool boosting;

    PhotonView myPV;

    private void Start()
    {
        boostController = GetComponent<BoostController>();
        isMoving = false;
        myPV = GetComponent<PhotonView>();
    }
    private void FixedUpdate()
    {
        if (myPV.IsMine)
        {
            if (SceneManager.GetActiveScene().name == "GameMap1")
            {
                if (GameMap1Controller.isStartedMatch)
                {
                    vertical = Input.GetAxis("Vertical");
                    horizontal = Input.GetAxis("Horizontal");
                    if (MyPlayerController.instance.KPH > 10)
                    {
                        isMoving = true;
                    }
                    else
                    {
                        isMoving = false;
                    }
                    handbrake = (Input.GetKey(KeyCode.Space) && MyPlayerController.instance.KPH > 30 && (horizontal > 0.8 || horizontal < -0.8)) ? true : false;
                    if (boostController.driftBoost || boostController.startBoost) boosting = true; else boosting = false;
                }
                else
                {
                    if (BoostController.instance.startBoostReady) boosting = true; else boosting = false;
                }
            }
            else
            {
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal");
                if (MyPlayerController.instance.KPH > 10)
                {
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }
                handbrake = (Input.GetKey(KeyCode.Space) && MyPlayerController.instance.KPH > 30 && (horizontal > 0.8 || horizontal < -0.8)) ? true : false;
                if (boostController.driftBoost || boostController.startBoost) boosting = true; else boosting = false;
            }
        }
    }    
}
