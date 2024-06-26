# Overview

## UI and Server Management

The user interface facilitates logic with the server and entry into the game with desired settings. There are two main scripts that control this flow: `CreateAndJoinRooms.cs` and `ConnectToServer`.

The server is managed by the Photon Network library. Custom properties and RPC calls are used to sync the game state between all the players in the same room. All the scripts featured in this README will interact with Photon in order to maintain the game state.

The `ConnectToServer.cs` script manages a player's entry into the server with a nickname. It configures the server to sync the scene for all players in the game:

```c#
if (NameInput.text.Length > 0)
{
    PhotonNetwork.NickName = NameInput.text;
    PhotonNetwork.ConnectUsingSettings();
    PhotonNetwork.AutomaticallySyncScene = true;
}

SceneManager.LoadScene("Lobby");
```

The `CreateAndJoinRooms.cs` script manages the lobby scene, player selection, and map selection.

The `CreateAndJoinPanel` allows the user to create a room or join an existing one.

```c#
PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { BroadcastPropsChangeToAll = true });

PhotonNetwork.JoinRoom(joinInput.text);
```

The `RoomPanel` displays players in the room and allows the local player to update his player. The creator of the room also has the option to change the map (opening the `MapSelectPanel`) and start the game for everyone in the room.

Here is the logic for how players are added to this room:

```c#
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
        if (player.Value.IsLocal)
        {
            // instantiate the player object in the room panel
            PlayerObject newPlayerObject = Instantiate(playerObjectPrefab, mainPlayerObjectParent);
            newPlayerObject.SetPlayerName(player.Value);
            newPlayerObject.ApplyLocalChanges();
            playerObjects.Add(newPlayerObject);
        }
        else
        {
            PlayerObject newPlayerObject = Instantiate(playerObjectPrefab, playerObjectParent);
            newPlayerObject.SetPlayerName(player.Value);
            playerObjects.Add(newPlayerObject);
        }
    }
}
```

The following code is run when the host selects 'Start Game'

```c#
PhotonNetwork.LoadLevel(mapName);
```

### Design

The design of the game utilized several asset sets, character asset sets, object asset sets, input assets, and background assets. the character assets objects and input assets were obtained from free or purchased assets from Unity while the background assets were created through COPIOLET AI generation and photo manipulation. COPIOLET was prompted for various pixel art background specifications and the images were refined until they matched a desired outcome. then they were taken into a photo editor to mirror aspects of the images to make them tileable. the backgrounds were then added to scenes and further adjusted to make them distinct from the scene objects. this might involve adding color changes in Unity or returning to the photo editor to make additional changes such as hue or saturation. A key aspect was matching the color scheme of the scene assets to the background but also making the background distinct from the scene assets to allow users to make the distinction.

The input assets such as forms, buttons, text inputs and fonts were made using simplePixelart asset set which is an asset of sprites and fonts that matched the design aspect of our game. In addition to the assets, some design aspects were done inside the unity editor such as color, gradients, and opacity.

## Maps

Cave
![Cave](./images/Cave.png)

Nirmata-Hidden
![Nirmata](./images/Nirmata.png)

DonkyKong
![DK](./images/DK.png)

NewYork
![NY](./images/NewYork.png)

Earth
![Earth](./images/Earth.png)

Gold
![Gold](./images/Gold.png)

All maps were created specialy for this project using free assets from the Unity Asset Store.

## Movement

### Movement Scripts

There are two scripts that control each player. More specifically PhotonView is used to determine which player is being control by a specific user. The CharacterController script works to handle specific events for the player such as flipping the character, crouching events, and jumping events. The PlayerMovement scripts works to take in the user inputs and handles the movement. It also works to call the player animations based on the inputs.
Look at the CharacterController script here: [Character Controller](Nirmata-Mahasura/Assets/Scripts/PlayerScripts/CharacterController2D.cs)
Look at the PlayerMovement script here:
[Player Movement](Nirmata-Mahasura/Assets/Scripts/PlayerScripts/PlayerMovement.cs)

### Movement Animations

Sprites for animations are free assets and were taken from itch.io. The animations play based on different parameters created in the unity animator. There are multiple animations implemented to each character including idle, run, jump, crouch, take damage, and die.

## Health and Damage

### Scripts

The Health and Damage scripts use simple math to upkeep the remaining health for any given player. The difficulty with these scripts comes with the fact that we are constantly trying to pass our updated health back and forth between every player. To do this we ended up using an RPC function that updates a users health across the other computers in the network.

```C#
[PunRPC]
    void UpdateHealthRPC(int health)
    {
        currentHealth = health;
        if (currentHealth > 0)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        else
        {
            Die();
        }
    }
```

This code is called every couple seconds, which is something that needs to be changed in future iterations of the project. The RPC call really only needs to be called every time something takes damage. Right now we are sending out blank RPC calls to every computer in the network every few seconds, which is causing a lot of jittering and slowness.

We have a bullet prefab that changes player health script when it impacts with the parent object. We simply subtract a set amount of damage from the health script then we call the above RPC function to update across the network.

```C#
 void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Health enemy = hitInfo.GetComponent<Health>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
```

### Health Bar

Tutorial Video
[Simple Health Bar](https://youtu.be/_lREXfAMUcE?si=X5_GZbss-xG3mH48)
[How to Make the Health Bar Interactable with A Character's Health](https://youtu.be/0tDPxNB2JNs?si=GqMcURyH8-TU7vBO)

In the HealthBar class, it only updates the health bar. You can call it in Health.cs file to display
the health change anytime like when a character gets damage or healed.

```C#
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.fillAmount = currentValue / maxValue;
    }
}
```

## Camera

The Camera Script allows the Camera to act using different functions. Most importantly, the Camera will attach itself to the "Active" player. The active player will always be the player being controlled, so on each computer the active player will be different. Once there is no more active player, the camera will switch to a stagnant position showing overview the whole map. This allows players who have been defeated to see the rest of the living players until the game eventually ends.

```C#
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

```

Connected to the idea of the Camera script is the UpdateTag script, which allows the game to figure out which mode the Camera should be set into. The UpdateTag script is applied to each player Prefab and tells the player to update their tag to "Player" when it detects that they are the host of their machine. This player tag is what signifies an active player to the Camera script.

```C#
void Start()
    {
        if(photonView.IsMine){
            gameObject.tag = "Player";
        }
    }
```

# Development Environment

This game was developed in Unity with the help of the Photon Network library, itch.io, and Photon.

# Collaborators

Luke Briggs - Team Leader - [Github](https://github.com/uplandwave)

Samuel Mickelsen - Documentation Manager - [Github](https://github.com/Sammickelsen)

Rai Katsuragawa - Configuration Manager - [Github](https://github.com/katsu-rai)

Gilber Chen - Project Manager - [Github](https://github.com/ooioioogt)

Kyle Guo - Graphic Designer - [Github](http://github.com/kyleguo123)

Blaine Freestone - Project Manager - [Github](https://github.com/blainefreestone)

James Call - Graphic Designer - [Github](https://github.com/levijohnson1227)

Levi Johnson - Levi Johnson - [Github](https://github.com/jacall2016)

* In addition to their own jobs the whole team worked on what needed to be done, not worrying about titles.

# Useful Websites

- [Unity Asset Store](https://assetstore.unity.com/)
- [Web Site Name](http://url.link.goes.here)

# Future Work

- Design: Add layered backgrounds and animations to the backgrounds, add button press animations
- Maps: Fix all tearing. Two way colliders.
