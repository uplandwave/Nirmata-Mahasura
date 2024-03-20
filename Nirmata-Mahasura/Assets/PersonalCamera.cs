using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PersonalCamera : MonoBehaviour
{
    public GameObject cameraPrefab;
    public PhotonView photonView;

    void Start()
    {
        if (photonView.IsMine)
        {
            // If this is the local player, spawn a camera for them
            PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity);
        }
    }
}

