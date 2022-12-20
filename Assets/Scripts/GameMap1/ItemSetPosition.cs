using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSetPosition : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView myPV;
    [Header("スプレッドシートの順で")]
    [SerializeField] GameObject[] Item;

    [SerializeField] ItemGetAndSet _itemGetAndSet;

    private void FixedUpdate()
    {
        if (!myPV.IsMine) return;

        //アイテムが一つ以上あるとき
        if (_itemGetAndSet.CanUseItem())
        {
            myPV.RPC(nameof(Active), RpcTarget.All);
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
