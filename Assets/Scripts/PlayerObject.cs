using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour
{
    
    public GameObject PlayerUnitPrefab;

    // Start is called before the first frame update
    void Start(){

        if (isLocalPlayer == false){
            return;
        }
        
        CmdSpawnMyUnit();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    [Command]
    void CmdSpawnMyUnit(){
        GameObject go = Instantiate(PlayerUnitPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
