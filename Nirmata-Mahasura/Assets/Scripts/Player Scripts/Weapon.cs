
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

    // private void Start()
    // {
    //     // Check if bulletPrefab and firePoint are assigned
    //     if (bulletPrefab == null)
    //     {
    //         Debug.LogError("bulletPrefab is not assigned in Weapon script.");
    //     }
    //     if (firePoint == null)
    //     {
    //         Debug.LogError("firePoint is not assigned in Weapon script.");
    //     }
    // }
   
    [PunRPC]
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            else
            {
                Debug.LogError("bulletPrefab or firePoint is null in Weapon script.");
            }
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
