using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UpdateTag : MonoBehaviour
{
    private PhotonView photonView;

    void Awake(){
        photonView = GetComponent<PhotonView>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine){
            gameObject.tag = "Player";
        }
    }
}
