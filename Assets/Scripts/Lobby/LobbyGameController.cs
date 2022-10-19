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
        SpawnSystem.instance.SpawnPlayerInLobby();
    }
}
