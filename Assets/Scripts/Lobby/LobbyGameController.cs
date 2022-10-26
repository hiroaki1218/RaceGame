using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LobbyGameController : MonoBehaviourPunCallbacks
{
    public static LobbyGameController instance;
    public Transform[] spawnPoints;

    private void Start()
    {
        instance = this;
        //Player Spawn
        StartCoroutine(SpawnDelay());
    }

    //ÉXÉ|Å[ÉìÇ‹Ç≈è≠Çµéûä‘Ç©ÇØÇÈ
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnSystem.instance.SpawnPlayerInLobby();
    }
}
