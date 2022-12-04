using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public BoostController boostController;
    public bool isMoving;
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool boosting;

    private void Start()
    {
        boostController = GetComponent<BoostController>();
        isMoving = false;
    }
    private void FixedUpdate()
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
                if (BoostController.instance.startBoostReady) boosting = true;else boosting = false;
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
