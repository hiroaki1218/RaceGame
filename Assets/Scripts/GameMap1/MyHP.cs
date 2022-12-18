using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MyHP : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float MaxHP = 100f;
    public float increaseHP = 4;

    [SerializeField] public Slider myHPBar = default;
    [SerializeField] public Slider toOtherHPBar = default;
    private float currentHP = MaxHP;
    public float takeDamage;

    PhotonView myPV;
    bool IsGetComponents;
    public bool isMyPlayerAvatar;

    //Component�̎擾
    public void GetComponentsHP()
    {
        IsGetComponents = false;
        myPV = GetComponent<PhotonView>();
        myHPBar = transform.Find("MyUI/HPAndShieldCanvas/Shield/ShieldBar/HP/HPBar").gameObject.GetComponent<Slider>();
        toOtherHPBar = transform.Find("toOtherUI/HPAndShieldCanvas/Shield/ShieldBar/HP/HPBar").gameObject.GetComponent<Slider>();
        IsGetComponents = true;
    }

    private void Update()
    {
        Mathf.Clamp(currentHP, 0, 100);
        if (!IsGetComponents) return;

        if (myPV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                takeDamage = 30;
            }

            if (takeDamage > 0)
            {
                // ���͂���������A�X�^�~�i������������
                currentHP = currentHP - takeDamage;
                if(currentHP <= 0)
                {
                    currentHP = 0;
                }
                takeDamage = 0;
            }
            else
            {
                // ���͂��Ȃ�������A�X�^�~�i���񕜂�����
                currentHP = currentHP + Time.deltaTime * increaseHP;
                if (currentHP >= MaxHP)
                {
                    currentHP = MaxHP;
                }
            }
           
        }

        // �X�^�~�i���Q�[�W�ɔ��f����
        myHPBar.value = currentHP / MaxHP;
        toOtherHPBar.value = currentHP / MaxHP;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���g�̃A�o�^�[��HP�𑗐M����
            stream.SendNext(currentHP);
        }
        else
        {
            // ���v���C���[�̃A�o�^�[��HP����M����
            currentHP = (float)stream.ReceiveNext();
        }
    }
}
