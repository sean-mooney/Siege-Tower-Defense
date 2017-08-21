using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    [SyncVar]
    private int players = 0;

    

    [ClientRpc]
    public void RpcSetPlayers(int connections)
    {
        players = connections;
    }

    public int GetPlayers()
    {
        return players;
    }

    [Command]
    public void CmdSetPlayerCount()
    {
        RpcSetPlayers(NetworkServer.connections.Count);
    }

    public void Update()
    {
        if (isServer)
        {
            if (players != NetworkServer.connections.Count)
            {
                CmdSetPlayerCount();
            }
        }
    }
}
