using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraFollow : MonoBehaviour
{
    public static MyCameraFollow instance;
    public MyPlayerController myPlayerController;
    public InputManager inputmanager;
    [SerializeField] Vector3 offset;
    public Transform target;
    private Camera mycamera;
    public float speed;
    public float localspeed;
    public float defaltFOV = 0,desiredFOV = 0;
    [Range(0,5)]public float smothTime = 0;
    public float speedchange;
    private bool first;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        first = true;
        mycamera = GetComponent<Camera>();
        defaltFOV = mycamera.fieldOfView;
        speedchange = 1;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        first = false;
    }

    private void FixedUpdate()
    {
        if (SpawnSystem.instance.isAllSpawned && myPlayerController != null)
        {
            speed = Mathf.Lerp(speed, myPlayerController.KPH / 2, Time.deltaTime);
            localspeed = speed;
        }

        //ドリフト時のカメラスピード調整
        if (target != null)
        {
            if (inputmanager.handbrake)
            {
                speedchange -= Time.deltaTime / 8;
                if(speedchange <= 0.6f)
                {
                    speedchange = 0.6f;
                }

                localspeed = speed * speedchange;
            }
            else
            {
                speedchange += Time.deltaTime / 7;
                if (speedchange >= 1f)
                {
                    speedchange = 1f;
                }
                localspeed = speed * speedchange;
            }
        }
           
        HandleTranslation();
        HandleRotation();

        boostFOV();
    }

    private void HandleTranslation()
    {
        if(target != null)
        {
            if(first)
            {
                StartCoroutine(Waiting());
                var targetPosition = target.TransformPoint(offset);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 20 * Time.deltaTime);
            }
            else
            {
                var targetPosition = target.TransformPoint(offset);
                transform.position = Vector3.Lerp(transform.position, targetPosition, localspeed * Time.deltaTime);
            } 
        }
    }

    private void HandleRotation()
    {
        if (target != null)
        {
            var direction = target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, localspeed * Time.deltaTime);
        }
    }

    private void boostFOV()
    {
        if(target != null)
        {
            if (inputmanager.boosting)
                mycamera.fieldOfView = Mathf.Lerp(mycamera.fieldOfView, desiredFOV, Time.deltaTime * smothTime);
            else
                mycamera.fieldOfView = Mathf.Lerp(mycamera.fieldOfView, defaltFOV, Time.deltaTime * smothTime);
        }
    }
}
