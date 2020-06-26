using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{
   
    void Start()
    {

    }

    public void OnClicktoMasterServer()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v1";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        base.OnCustomAuthenticationFailed(debugMessage);
    }
   
}
