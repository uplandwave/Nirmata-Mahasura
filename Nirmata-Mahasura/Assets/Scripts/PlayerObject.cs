using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerObject : MonoBehaviour
{
    public TMP_Text PlayerName;

    public void SetPlayerName(Player _player)
    {
        PlayerName.text = _player.NickName;
    }
}
