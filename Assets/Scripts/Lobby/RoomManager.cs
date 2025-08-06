using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using JetBrains.Annotations;
using UnityEngine.Rendering;
using TMPro;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public GameObject EndgameUI;
   
    // Start is called before the first frame update
    [Header("Player Object")]
    public GameObject player;

    [Header("Player Spawn Point")]
    public Transform[] spawnPoints;

    [Header("Free Look Camera")]
    public CinemachineFreeLook freeLook;

    [Header("Camera UI")]
    public GameObject roomCam;

    [Header("UI")]
    public GameObject nickNameUI;
    public Button JoinButton;
    
    public GameObject playerListUI;

    [Header("Connecting Screen")]
    public GameObject connectingUI;
    public TextMeshProUGUI no_of_players;
    public GameObject startGameButton;

    [Header("Room Name")]
    public string roomName = "test";

    string nickName = "unnamed";
    [Header("Player Color")]
    public Material[] playerColors;
    [Header("RoomData")]
    public RoomData roomData;
    [HideInInspector]
    public int kills;
    [HideInInspector]
    public int deaths;
    [HideInInspector]
    public int damage;
    public bool Joined = false;
    public bool game_Started = false;
    public bool Host = false;
    private void Awake()
    {
        Instance = this;
        
    }
    public void IsHost()
    {
        Host = true;
    }
    private void Start()
    {
        no_of_players.text = "1/16";
        roomData = new RoomData(roomName, 0, 16, false, 300f);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetComponent<PhotonView>().RPC("playerchanged", RpcTarget.AllBuffered);
        Debug.Log("Player Left Room: " + otherPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<SoundManager>().PlaySound(2);
        }
        if (game_Started)
        {
            FindObjectOfType<JOINNOTIFICATION>().LeftNotification(PhotonNetwork.LocalPlayer.NickName);
        }
        Joined = false;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        Debug.Log("Player Joined Room: " + newPlayer.NickName);
        GetComponent<PhotonView>().RPC("playerchanged", RpcTarget.AllBuffered);


    }
    [PunRPC]
    public void playerchanged()
    {
        no_of_players.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/16";
    }
    public void SetNickname(string _name)
    {
        nickName = _name;
        
    }
    public void OnCreateButtonPressed()
    {
        if (!Host)
        {
            Debug.Log("Not the Master Client, cannot create a room.");
            return; // Only the master client can create a room
        }
        Debug.Log("Creating Room. . . ");
        Debug.Log(roomName);
        var opts = new RoomOptions
        {
            MaxPlayers = 16,       // anything > 0 makes the room visible
            IsVisible  = true,     // (true by default, but set it anyway)
            IsOpen     = true      // allow others to join
        };
        PhotonNetwork.CreateRoom(roomName, opts, TypedLobby.Default);
        
        nickNameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    public void OnJoinButtonPressed()
    {
        if (Host) {
            Debug.Log("Already the Master Client, cannot join another room.");
            return; // Prevent joining if already the master client
        }
        Debug.Log(message: "Connecting. . . ");
        Debug.Log(roomName);
        PhotonNetwork.JoinRoom(roomName);


        nickNameUI.SetActive(false);
        connectingUI.SetActive(true);
        
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }



    }
    public void StartButtonPressed()
    {
        
        Debug.Log("Start Button Pressed");
        if (Host)
        {
            Debug.Log("Master Client Starting Game");
            PhotonNetwork.CurrentRoom.IsOpen = false; // Prevent new players from joining
            PhotonNetwork.CurrentRoom.IsVisible = false; // Hide the room from the lobby
            //PhotonNetwork.LoadLevel("GameScene"); // Load the game scene
            GetComponent<PhotonView>().RPC("StartGameRPC", RpcTarget.AllBuffered);
        }
        Time.timeScale = 1f;
    }
    [PunRPC]
    public void StartGameRPC()
    {
        
        Debug.Log("Starting Game");
        roomCam.SetActive(false);
        EndgameUI.SetActive(true);
        FindObjectOfType<SoundManager>().PlaySound(1);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Debug.Log("Spawning Player");
        
        
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        Material material = playerColors[UnityEngine.Random.Range(0, playerColors.Length)];
        SkinnedMeshRenderer smr = _player.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
        Material[] sharedMats = smr.sharedMaterials;
        sharedMats[0] = material;
        smr.sharedMaterials = sharedMats;
        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;
        
        Debug.Log(_player.name);

        PhotonView view = _player.GetComponent<PhotonView>();
        view.RPC("SetPlayerName", RpcTarget.AllBuffered, nickName);
        PhotonNetwork.LocalPlayer.NickName = nickName;

        if (!Joined)
        {
           
            FindObjectOfType<JOINNOTIFICATION>().ShowNotification(PhotonNetwork.LocalPlayer.NickName);
            Joined = true;
            
        }
        if (view != null && view.IsMine && freeLook != null)
        {
            Transform lookAt = _player.transform.GetChild(1);
            freeLook.Follow = lookAt;
            freeLook.LookAt = lookAt;
        }
    }
     
   
    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties; // Get the custom properties of the local player
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            hash["damage"] = damage;
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
        catch
        {

        }
    }
    public void OnRestartButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<PhotonView>().RPC("RestartGameRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void RestartGameRPC()
    {
        // Reset all relevant game state variables and objects here.
        // For example:
        kills = 0;
        deaths = 0;
        damage = 0;
        
        // Optionally, despawn and respawn all players (or just respawn them at spawn points)
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(playerObj); // or implement logic to reset health, score, etc.
        }

        //SpawnPlayer();
        FindObjectOfType<endmanager>().duration = FindObjectOfType<endmanager>().timerdur; // Reset the game duration
        // Reset or hide/display UI as needed
        EndgameUI.transform.GetChild(0).gameObject.SetActive(false);
        roomCam.SetActive(true);
        // ...Reset any other needed state/UI elements
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && nickNameUI.activeInHierarchy)
        {
            JoinButton.onClick.Invoke();
        }

    }

}
