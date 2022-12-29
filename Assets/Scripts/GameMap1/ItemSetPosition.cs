using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ItemSetPosition : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject[] ItemObjects;
    public ItemGetAndSet itemgetandset;
    public ItemReelController itemreelcontroller;
    int itemnum;
    private void Start()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (SceneManager.GetActiveScene().name == "Menu") return;
        if (!itemgetandset.gotItemFirst) return;

        itemnum = (int)itemgetandset.itemInSlot[0].type;
        if (itemreelcontroller.satItemSlot1)
        {
            photonView.RPC(nameof(SetActiveTrueItem), RpcTarget.All);
        }
        

        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if(i != itemnum)
            {
                photonView.RPC(nameof(SetActiveFalseItem), RpcTarget.All,i);  
            }
        } 
    }

    [PunRPC]
    void SetActiveTrueItem()
    {
        ItemObjects[itemnum].SetActive(true);
    }

    [PunRPC]
    void SetActiveFalseItem(int i)
    {
        ItemObjects[i].SetActive(false);
    }
}
