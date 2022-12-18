using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSetPosition : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView myPV;
    [SerializeField] GameObject thisItem;
    [SerializeField] Items.Type type;
    [SerializeField] ItemGetAndSet _itemGetAndSet;

    private void FixedUpdate()
    {
        if (!myPV.IsMine) return;

        //�A�C�e������ȏ゠��Ƃ�
        if (_itemGetAndSet.CanUseItem())
        {
            //�����̃A�C�e���̃^�C�v�Ǝg����A�C�e���̃^�C�v����v���Ă���Ƃ�
            if(type == _itemGetAndSet.itemInSlot[0].type)
            {
                myPV.RPC(nameof(Active), RpcTarget.All);
            }
            else
            {
                //��v���ĂȂ��Ƃ�
                myPV.RPC(nameof(ActiveFalse), RpcTarget.All);
            }
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
        thisItem.SetActive(true);
    }

    [PunRPC]
    void ActiveFalse()
    {
        thisItem.SetActive(false);
    }
}
