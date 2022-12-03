using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if(MyPlayerController.instance.KPH > 10)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        handbrake = (Input.GetKey(KeyCode.Space) && MyPlayerController.instance.KPH > 30 && (horizontal>0.8 || horizontal<-0.8)) ? true : false;
        if (Input.GetKey(KeyCode.LeftShift) || boostController.driftBoost || boostController.startBoost) boosting = true; else boosting = false;
    }
}
