using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public float vertical;
    public float horizontal;
    public bool handbreak;
    public bool boosting;

    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbreak = (Input.GetKey(KeyCode.Space) && MyPlayerController.instance.KPH > 30) ? true : false;
        if (Input.GetKey(KeyCode.LeftShift)) boosting = true; else boosting = false;
    }
}
