using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoomData
{
    // Start is called before the first frame update
    public string roomName;
    public int playerCount;
    public int maxPlayers;
    public bool isGameStarted;
    public float gameTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public RoomData(string roomName, int playerCount, int maxPlayers, bool isGameStarted, float gameTime)
    {
        this.roomName = roomName;
        this.playerCount = playerCount;
        this.maxPlayers = maxPlayers;
        this.isGameStarted = isGameStarted;
        this.gameTime = gameTime;
    }
}
