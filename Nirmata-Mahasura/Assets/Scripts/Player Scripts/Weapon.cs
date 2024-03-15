
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public PhotonView view;

    [PunRPC]
    void Shoot()
    {

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);


    }
    // Update is called once per frame
    void Update()
    {

        if (view.IsMine && Input.GetButtonDown("Fire1"))
        {
            view.RPC("Shoot", RpcTarget.All);
        }

    }
}