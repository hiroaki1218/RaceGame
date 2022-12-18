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

        //アイテムが一つ以上あるとき
        if (_itemGetAndSet.CanUseItem())
        {
            //自分のアイテムのタイプと使えるアイテムのタイプが一致しているとき
            if(type == _itemGetAndSet.itemInSlot[0].type)
            {
                myPV.RPC(nameof(Active), RpcTarget.All);
            }
            else
            {
                //一致してないとき
                myPV.RPC(nameof(ActiveFalse), RpcTarget.All);
            }
        }
        else
        {
            //アイテムがないとき
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
