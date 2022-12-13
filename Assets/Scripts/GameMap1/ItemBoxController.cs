using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxController : MonoBehaviourPunCallbacks
{
    [SerializeField] ItemGetAndSet targetPlayer;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            targetPlayer = other.gameObject.GetComponent<ItemGetAndSet>();
            targetPlayer.GetItem();

            photonView.RPC(nameof(TouchItemBox), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void TouchItemBox()
    {
        this.gameObject.SetActive(false);
    }
}
