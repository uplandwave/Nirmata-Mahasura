using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Camera_Controller : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0f,0f,-10f);
  
    void Update()
    {
        FindPlayer();
        transform.LookAt(target);
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void FindPlayer()
    {
        GameObject findPlayer;
        findPlayer = GameObject.FindGameObjectWithTag("Player");     
        target = findPlayer.transform;
    }
}