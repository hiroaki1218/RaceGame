using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public bool startBoost;

    public bool canjustStartDash;
    private bool justStartDash;
    private bool ispushing;

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
    }

    private void Update()
    {
        //�X�^�[�g�_�b�V��
        if(SceneManager.GetActiveScene().name == "GameMap1")
        {
            //Case1:�X�^�[�g�_�b�V�����ł�����
            if (canjustStartDash && !ispushing)
            {
                //Case2:�X�^�[�g�_�b�V�����ł����Ԃ����؂��ȃ^�C�~���O�̎�
                if (Input.GetKey(KeyCode.Space))
                {
                    justStartDash = true;
                }
                else
                {
                    justStartDash = false;
                }
            }
            else
            {
                //Case3:�X�^�[�g�_�b�V�����ł��Ȃ����
                //Case4:�X�^�[�g�_�b�V�����ł����Ԃ��Ⴄ�^�C�~���O�̎�
                if (Input.GetKey(KeyCode.Space))
                {
                    ispushing = true;
                }
                else
                {
                    ispushing = false;
                }
            }

            //�����X�^�[�g�_�b�V�����������Ă�����A�u�[�X�g����
            if (justStartDash && GameMap1Controller.startboosttime)
            {
                myrigidbody.AddForce(transform.forward * 1800);
                startBoost = true;
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
