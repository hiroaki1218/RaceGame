using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public enum Axel
{
    Front,
    Back,
}

[System.Serializable]
public struct CarWheel
{
    public WheelCollider collider;
    public GameObject mesh;
    public Axel axel;
}

public enum DriveTrain
{
    Four_Wheel,
    Two_Wheel
}


public class MyPlayerController : MonoBehaviourPunCallbacks
{
    public static MyPlayerController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }

    [SerializeField] private driveType drive;

    private InputManager inputManager;
    public WheelCollider[] wheels = new WheelCollider[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    [Header("Varriables")]
    public float handBrakeFrictionMultiplier = 2f;
    //public float totalPower;
    //public AnimationCurve enginePower;
    //public float engineRPM;
    public float maxEnginePower;
    public float enginePower;
    public float wheelsRPM;
    public float smothTime = 0.01f;
    //public float[] gears = new float[5];
    //[Range(0,4)]public int gearNum = 0;

    public float KPH;
    public float brakePower;
    public float thrust = 1000;
    public float radius = 6;
    public float DownForceValue = 100;
    public float steeringMax = 4;

    [Header("DEBUG")]
    public float[] slip = new float[4];

    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    public bool onGround;

    PhotonView myPV;

    private void Start()
    {
        getObjects();
        StartCoroutine(timedLoop());
        onGround = false;
        myPV = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (!myPV.IsMine) return;
        moveVehicle();
        steerVehicle();
        addDownForce();
        getFriction();
        //calculateEnginePower();
        //shifter();
        adjustTraction();
        checkWheelSpin();
        deltaenginePower();
    }


    private void deltaenginePower()
    {
        if(KPH > 100)
        {
            enginePower = enginePower / 2;
        }
        else
        {
            enginePower = maxEnginePower;
        }
    }
    //private void calculateEnginePower()
    //{
    //wheelRPM();
    //totalPower = enginePower.Evaluate(engineRPM) * (gears[gearNum]) * inputManager.vertical;
    //if(totalPower <= 0)
    //{
    //totalPower = -totalPower;
    //}
    //float velocity = 0.0f;
    //engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity, smothTime);
    //}

    private void wheelRPM()
    {
        float sum = 0;
        int r = 0;
        for(int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            r++;
        }
        wheelsRPM = (r != 0) ? sum / r : 0;
    }

    //private void shifter()
    //{
        //if (Input.GetKeyDown(KeyCode.E))
        //{
            //gearNum++;
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
            //gearNum--;
        //}
    //}

    //Carの動き
    private void moveVehicle()
    {
        //4駆
        if (drive == driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (enginePower / 4);
            }
        }
        else if (drive == driveType.rearWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (enginePower / 2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (enginePower / 2);
            }
        }

        //速度計算
        KPH = rigidbody.velocity.magnitude * 3.6f;

    }

    private void steerVehicle()
    {
        if(inputManager.horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.25f / (radius + (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.25f / (radius - (1.5f / 2))) * inputManager.horizontal;
        }
        else if(inputManager.horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.25f / (radius - (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.25f / (radius + (1.5f / 2))) * inputManager.horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[0].steerAngle = 1;
        }
    }

    //InputManagerを取得
    private void getObjects()
    {
        inputManager = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody>();
        centerOfMass = this.transform.Find("mass").gameObject;
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void addDownForce()
    {
        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);
    }

    private void getFriction()
    {
        for(int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            slip[i] = wheelHit.forwardSlip;

            if(wheelHit.collider == null)
            {
                onGround = false;
            }
            else if (wheelHit.collider.tag == "Road")
            {
                onGround = true;
            }  
        }
    }

    private float driftFactor;
    private void adjustTraction()
    {
        float driftsmothFactor = .7f * Time.deltaTime;

        if (inputManager.handbrake)
        {
            sidewaysFriction = wheels[0].sidewaysFriction;
            forwardFriction = wheels[0].forwardFriction;

            float velocity = 0;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = Mathf.SmoothDamp(sidewaysFriction.asymptoteValue, driftFactor, ref velocity, driftsmothFactor);
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftFactor, ref velocity, driftsmothFactor);

            for (int i = 0; i < 4; i++)
            {
                wheels[i].forwardFriction = forwardFriction;
                wheels[i].sidewaysFriction = sidewaysFriction;
            }

            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = 1.1f;
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = 1.1f;

            for (int i = 0; i < 2; i++)
            {
                wheels[i].forwardFriction = forwardFriction;
                wheels[i].sidewaysFriction = sidewaysFriction;
            }
            //AddForce
            rigidbody.AddForce(transform.forward * -(KPH - 120) * 100);
            rigidbody.AddForce(-transform.forward * -(KPH - 120) * 30);

            if (inputManager.horizontal > 0)
            {
                rigidbody.AddForce(-transform.right * -(KPH - 120) * 120);
            }
            else
            {
                rigidbody.AddForce(transform.right * -(KPH - 120) * 120);
            }
        }
        else
        {

            forwardFriction = wheels[0].forwardFriction;
            sidewaysFriction = wheels[0].sidewaysFriction;

            forwardFriction.extremumValue = forwardFriction.asymptoteValue = ((KPH * handBrakeFrictionMultiplier) / 300) + 1;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = ((KPH * handBrakeFrictionMultiplier) / 300) + 1;

            for (int i = 0; i < 4; i++)
            {
                wheels[i].forwardFriction = forwardFriction;
                wheels[i].sidewaysFriction = sidewaysFriction;
            }
        }
    }

    private float tempo;
    private void checkWheelSpin()
    {
        float blind = 0.28f;

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
            //rigidbody.AddForce(transform.forward * thrust);
        //}
        if (inputManager.handbrake)
        {
            for(int i = 0; i < 4; i++)
            {
                WheelHit wheelHit;
                wheels[i].GetGroundHit(out wheelHit);
                if (wheelHit.sidewaysSlip > blind || wheelHit.sidewaysSlip < -blind)
                {
                    //applyBooster(wheelHit.sidewaysSlip);
                }
            }
        }

        for(int i = 2; i < 4; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            if(wheelHit.sidewaysSlip < 0)
            {
                tempo = (1 + -inputManager.horizontal) * Mathf.Abs(wheelHit.sidewaysSlip * handBrakeFrictionMultiplier);
                if (tempo < 0.5) tempo = 0.5f;
            }
            if (wheelHit.sidewaysSlip > 0)
            {
                tempo = (1 + inputManager.horizontal) * Mathf.Abs(wheelHit.sidewaysSlip * handBrakeFrictionMultiplier);
                if (tempo < 0.5) tempo = 0.5f;
            }
            if (wheelHit.sidewaysSlip > 0.99f || wheelHit.sidewaysSlip < -0.99f)
            {
                float velocity = 0;
                driftFactor = Mathf.SmoothDamp(driftFactor,tempo * 3,ref velocity,0.1f * Time.deltaTime);
            }
            else
            {
                driftFactor = tempo;
            }
        }
    }

    private IEnumerator timedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;
        }
    }

    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "レースタイム",
                    Value = playerScore
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {playerScore} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
