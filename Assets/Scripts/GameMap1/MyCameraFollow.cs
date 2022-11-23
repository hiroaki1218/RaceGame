using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraFollow : MonoBehaviour
{
    public static InputManager inputmanager;
    [SerializeField] Vector3 offset;
    public static Transform target;
    private Camera mycamera;
    public float speed;
    public float defaltFOV = 0,desiredFOV = 0;
    [Range(0,5)]public float smothTime = 0;

    private void Start()
    {
        mycamera = GetComponent<Camera>();
        defaltFOV = mycamera.fieldOfView;
    }

    private void FixedUpdate()
    {
        if (SpawnSystem.instance.isAllSpawned)
        {
            speed = Mathf.Lerp(speed, MyPlayerController.instance.KPH / 2, Time.deltaTime);
        }
        
        HandleTranslation();
        HandleRotation();

        boostFOV();
    }

    private void HandleTranslation()
    {
        if(target != null)
        {
            var targetPosition = target.TransformPoint(offset);
            if(!inputmanager.handbreak)
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed / 1.6f * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (target != null)
        {
            var direction = target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            if (!inputmanager.handbreak)
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed / 1.6f * Time.deltaTime);
        }
    }

    private void boostFOV()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            mycamera.fieldOfView = Mathf.Lerp(mycamera.fieldOfView, desiredFOV, Time.deltaTime * smothTime);
        else
            mycamera.fieldOfView = Mathf.Lerp(mycamera.fieldOfView, defaltFOV, Time.deltaTime * smothTime);
    }
}
