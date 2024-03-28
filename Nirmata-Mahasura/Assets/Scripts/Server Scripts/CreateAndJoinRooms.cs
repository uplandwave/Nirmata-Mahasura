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
    // panels
    public GameObject CreateAndJoinPanel;
    public GameObject RoomPanel;
    public GameObject MapSelectPanel;

    // create and join room objects
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text roomName;
    public TMP_Text playerName;

    // room listing objects
    public List<PlayerObject> playerObjects = new List<PlayerObject>();
    public PlayerObject playerObjectPrefab;
    public Transform playerObjectParent;
    public GameObject startButton;
    public GameObject mapSelectEnableButton;

    // map select objects
    public Image mapImage;
    public Image smallMapImage;
    public Sprite[] maps;
    public string[] mapNames;
    public GameObject rightArrowButton;
    public GameObject leftArrowButton;
    public GameObject selectButton;
    ExitGames.Client.Photon.Hashtable mapProperties = new ExitGames.Client.Photon.Hashtable();

    public int mapIndex;
    public string mapName;

    private void Start()
    {
        CreateAndJoinPanel.SetActive(true);
        RoomPanel.SetActive(false);
        MapSelectPanel.SetActive(false);
        mapIndex = 0;
        mapProperties["mapIndex"] = mapIndex;
        UpdateMapImage();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!MapSelectPanel.activeSelf)
            {
                mapSelectEnableButton.SetActive(true);
                startButton.SetActive(true);
            }
        }
        else
        {
            startButton.SetActive(false);
            mapSelectEnableButton.SetActive(false);
        }
    }

    public void OnCreateRoomButton()
    {
        if (createInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { BroadcastPropsChangeToAll = true });
            CreateAndJoinPanel.SetActive(false);
            MapSelectPanel.SetActive(false);
            RoomPanel.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        CreateAndJoinPanel.SetActive(false);
        MapSelectPanel.SetActive(false);
        RoomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public void OnClickedPlayGameButton()
    {
        PhotonNetwork.LoadLevel(mapName);
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

            if (player.Value.IsLocal)
            {
                newPlayerObject.ApplyLocalChanges();
            }

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

    public void OnClickRightArrowMapSelect()
    {
        if (mapIndex == maps.Length - 1)
        {
            mapIndex = 0;
        }
        else
        {
            mapIndex++;
        }
        UpdateMapImage();
    }

    public void OnClickLeftArrowMapSelect()
    {
        if (mapIndex == 0)
        {
            mapIndex = maps.Length - 1;
        }
        else
        {
            mapIndex--;
        }
        UpdateMapImage();
    }

    public void OnClickSelectMap()
    {
        MapSelectPanel.SetActive(false);
        startButton.SetActive(true);
        mapSelectEnableButton.SetActive(true);
        smallMapImage.gameObject.SetActive(true);
        mapProperties["mapIndex"] = mapIndex;
    }

    public void UpdateMapImage()
    {
        mapImage.sprite = maps[mapIndex];
        smallMapImage.sprite = maps[mapIndex];
        mapName = mapNames[mapIndex];
    }

    public void OnMapEnableClick()
    {
        mapSelectEnableButton.SetActive(false);
        startButton.SetActive(false);
        MapSelectPanel.SetActive(true);
        smallMapImage.gameObject.SetActive(false);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("mapIndex"))
        {
            mapIndex = (int)propertiesThatChanged["mapIndex"];
            UpdateMapImage();
        }
    }
}
