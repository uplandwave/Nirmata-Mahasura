using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics.Contracts;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // create and join room objects
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public GameObject CreateAndJoinPanel;
    public GameObject RoomPanel;
    public TMP_Text roomName;
    public TMP_Text playerName;

    // room listing objects
    public List<PlayerObject> playerObjects = new List<PlayerObject>();
    public PlayerObject playerObjectPrefab;
    public Transform playerObjectParent;

    public void CreateRoom()
    {
        if (createInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }

    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        CreateAndJoinPanel.SetActive(false);
        RoomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        playerName.text = "Player Name " + PhotonNetwork.NickName;
       //  PhotonNetwork.LoadLevel("Game");
    }

    public void UpdatePlayerList()
    {
        // clear the current list of players
        foreach (PlayerObject player in playerObjects)
        {
            Destroy(player.gameObject);
        }
        playerObjects.Clear();

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerObject newPlayerObject = Instantiate(playerObjectPrefab, playerObjectParent);
            newPlayerObject.SetPlayerName(player.Value);
            playerObjects.Add(newPlayerObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
}
