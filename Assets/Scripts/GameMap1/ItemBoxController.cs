using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxController : MonoBehaviourPunCallbacks
{
    [SerializeField] ItemGetAndSet targetPlayer;


    //Colliderにプレイヤーが当たったとき
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            targetPlayer = other.gameObject.GetComponent<ItemGetAndSet>();
            //Playerのアイテムをランダムに決定
            targetPlayer.GetItem();

            //Boxを消す
            photonView.RPC(nameof(TouchItemBox), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void TouchItemBox()
    {
        this.gameObject.SetActive(false);
    }
}
