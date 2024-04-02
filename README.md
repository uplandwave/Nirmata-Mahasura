# Overview

## UI

## Maps

## Animations

## Health and Damage

## Camera

The Camera Script allows the Camera to act using different functions.  Most importantly, the Camera will attach itself to the "Active" player.  The active player will always be the player being controlled, so on each computer the active player will be different.  Once there is no more active player, the camera will switch to a stagnant position overviewing the whole map.  This allows players who have been defeated to see the rest of the living players until the game eventually ends.

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

Connected to the idea of the Camera script is the UpdateTag script, which allows the game to figure out which mode the Camera should be set into.  The UpdateTag script is applied to each player Prefab and tells the player to update their tage to "Player" when it detects that they are the host of their machine.  This player tag is what signifies an active player to the Camera script.

```C#
void Start()
    {
        if(photonView.IsMine){
            gameObject.tag = "Player";
        }
    }
```

## Server Management

{Important!  Do not say in this section that this is college assignment.}

{Provide a description of your team project.  Describe how to use the software.}

# Development Environment

{Describe the tools that you used to develop the software}

{Describe the programming language that you used and any libraries.}

# Collaborators

{Provide a list of everyone on your team}

# Useful Websites

{Make a list of websites that you found helpful in this project}
* [Web Site Name](http://url.link.goes.here)
* [Web Site Name](http://url.link.goes.here)

# Future Work

{Make a list of things that you need to fix, improve, and add in the future.}
* Item 1
* Item 2
* Item 3

