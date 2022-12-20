using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSetPosition : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView myPV;
    [Header("�X�v���b�h�V�[�g�̏���")]
    [SerializeField] GameObject[] Item;

    [SerializeField] ItemGetAndSet _itemGetAndSet;

    private void FixedUpdate()
    {
        if (!myPV.IsMine) return;

        //�A�C�e������ȏ゠��Ƃ�
        if (_itemGetAndSet.CanUseItem())
        {
            myPV.RPC(nameof(Active), RpcTarget.All);
        }
        else
        {
            //�A�C�e�����Ȃ��Ƃ�
            myPV.RPC(nameof(ActiveFalse), RpcTarget.All);
        }
    }

    [PunRPC]
    void Active()
    {
        int itemNum = (int)_itemGetAndSet.itemInSlot[0].type;
        Item[itemNum].SetActive(true);

        for (int i = 0; i < Item.Length; i++)
        {
            if(i != itemNum)
            {
                Item[i].SetActive(false);
            }
        }
    }

    [PunRPC]
    void ActiveFalse()
    {
        for(int i = 0; i < Item.Length; i++)
        {
            Item[i].SetActive(false);
        } 
    }
}
