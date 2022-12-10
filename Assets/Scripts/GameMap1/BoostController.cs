using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

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
    public float maxdrifttime = 2.5f;
    public bool driftBoost;
    public bool startBoost;

    public bool canjustStartDash;
    public bool startBoostReady;
    private bool justStartDash;
    private bool ispushing;

    PhotonView myPV;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        canjustStartDash = false;
        ispushing = false;
        justStartDash = false;
        mydriftUI.SetActive(false);
        myrigidbody = GetComponent<Rigidbody>();
        driftBoost = false;
        startBoost = false;
        startBoostReady = false;
        myPV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!myPV.IsMine) return;
        
        //�X�^�[�g�_�b�V��
        if(SceneManager.GetActiveScene().name == "GameMap1")
        {
            //Case1:�X�^�[�g�_�b�V�����ł�����
            if (canjustStartDash && !ispushing)
            {
                //Case2:�X�^�[�g�_�b�V�����ł����Ԃ����؂��ȃ^�C�~���O�̎�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                {
                    justStartDash = true;
                    startBoostReady = true;
                }
                else
                {
                    justStartDash = false;
                    startBoostReady = false;
                }
            }
            else if(!GameMap1Controller.isStartedMatch)
            {
                //Case3:�X�^�[�g�_�b�V�����ł��Ȃ����
                //Case4:�X�^�[�g�_�b�V�����ł����Ԃ��Ⴄ�^�C�~���O�̎�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                {
                    ispushing = true;
                    startBoostReady = true;
                }
                else
                {
                    ispushing = false;
                    startBoostReady = false;
                }
            }
            else
            {
                startBoostReady = false;
            }

            //�����X�^�[�g�_�b�V�����������Ă�����A�u�[�X�g����
            if (justStartDash && GameMap1Controller.startboosttime)
            {
                myrigidbody.AddForce(transform.forward * 1800);
                startBoost = true;
                startBoostReady = false;
            }
            else
            {
                startBoost = false;
            }
        }


        //�h���t�g�u�[�X�g�̌v�Z
        if(inputmanager != null)
        {
            if (inputmanager.handbrake)
            {
                mydriftUI.SetActive(true);

                driftTime += Time.deltaTime;
                driftgauge.value = driftTime / maxdrifttime;
            }
            else
            {
                mydriftUI.SetActive(false);

                if (driftTime > maxdrifttime)
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
            myrigidbody.AddForce(transform.forward * 1000);
        }
    }

    IEnumerator DriftBoostTimer()
    {
        yield return new WaitForSeconds(1f);
        currentdriftCount = 0;
        driftBoost = false;
    }
}
