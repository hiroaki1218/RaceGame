using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    public static BoostController instance;
    public InputManager inputmanager;
    [SerializeField] GameObject mydriftUI;
    [SerializeField] Slider driftgauge;
    public Rigidbody myrigidbody;
    public float driftCount = 1;
    public float currentdriftCount = 0;
    public float driftTime;
    public bool driftBoost;

    private bool drifting;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        mydriftUI.SetActive(false);
        myrigidbody = GetComponent<Rigidbody>();
        driftBoost = false;
    }

    private void Update()
    {
        //ドリフトブーストの計算
        if(inputmanager != null)
        {
            if (inputmanager.handbrake)
            {
                mydriftUI.SetActive(true);

                driftTime += Time.deltaTime;
                driftgauge.value = driftTime / 2.5f;
            }
            else
            {
                mydriftUI.SetActive(false);

                if (driftTime > 2.5f)
                {
                    if (currentdriftCount < driftCount)
                    {
                        driftTime = 0;
                        currentdriftCount++;
                        driftBoost = true;

                        StartCoroutine(DriftBoostTimer());
                    }
                }

                driftTime = 0;
            }
        }

        //DriftBoost
        if (driftBoost)
        {
            myrigidbody.AddForce(transform.forward * 1800);
        }
    }

    IEnumerator DriftBoostTimer()
    {
        yield return new WaitForSeconds(1f);
        currentdriftCount = 0;
        driftBoost = false;
    }
}
