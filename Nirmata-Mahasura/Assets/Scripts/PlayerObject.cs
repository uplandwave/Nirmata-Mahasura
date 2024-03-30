using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerObject : MonoBehaviourPunCallbacks
{
    public Vector3 fullScale = new Vector3(1.3f, 1.3f, 1.3f);

    public TMP_Text PlayerName;

    Image backgroundImage;
    public Color highlightColor;
    public GameObject rightArrowButton;
    public GameObject leftArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerName(Player _player)
    {
        PlayerName.text = _player.NickName;
        player = _player;
        UpdatePlayerObject(player);
    }

    public void ApplyLocalChanges()
    {
        // backgroundImage.color = highlightColor;
        rightArrowButton.SetActive(true);
        leftArrowButton.SetActive(true);
        transform.localScale = fullScale;
        transform.localPosition = new Vector2(0, 0);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        } else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerObject(targetPlayer);
        }
    }

    void UpdatePlayerObject(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }
}
