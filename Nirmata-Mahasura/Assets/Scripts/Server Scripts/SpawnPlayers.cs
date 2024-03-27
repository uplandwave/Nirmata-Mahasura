using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject[] spawnPoints;

    public void Start()
    {
        // get the random spawn point from the array of spawn points
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        // get vector position of the spawn point
        Vector2 spawnPosition = spawnPoints[spawnPointIndex].transform.position;

        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition, Quaternion.identity);
    }
}
