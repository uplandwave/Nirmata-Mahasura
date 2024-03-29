using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Camera_Controller : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0f,0f,-10f);

    private bool player = false;

    public float playerSize;

    public float centerSize;

    [SerializeField] private Camera cam;
  
    void Update()
    {
        FindPlayer();
        transform.LookAt(target);
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void FindPlayer()
    {
        try
        {
        GameObject findPlayer;
        findPlayer = GameObject.FindGameObjectWithTag("Player");     
        target = findPlayer.transform;
        cam.orthographicSize = playerSize;
        }
        catch
        {
            GameObject findCenter;
            findCenter = GameObject.FindGameObjectWithTag("Center");  
            target = findCenter.transform;
            cam.orthographicSize = centerSize;  
        }
    }
}