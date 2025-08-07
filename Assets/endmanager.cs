using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class endmanager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public float duration = 300f;
    public GameObject EndUI;
    public GameObject[] slots;
    public TextMeshProUGUI[] scoretexts;
    public TextMeshProUGUI[] nametexts;
    public TextMeshProUGUI[] kdtexts;
    public bool timerstarted = false;
    public TextMeshProUGUI timertext;
    public float timerdur;
    private void Start()
    {
        timer();
        timerdur = duration;
        
    }
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length>=2 && !timerstarted)
        {
            
            timerstarted = true; // Ensure this only runs once

        }
        if (timerstarted && duration>=0)
        {
            duration -= Time.deltaTime;
            timer();
        }
        if (duration <= 0)
        {
            EndGame();
            duration = 0; // Ensure duration does not go negative
        }



    }
    private void EndGame()
    {
        GetComponent<PhotonView>().RPC("EndGameRPC", RpcTarget.All);
    }
    // Update is called once per frame
    [PunRPC]
    public void EndGameRPC()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }
        Time.timeScale = 0f;
        EndUI.SetActive(true);
        var sortedPlayerslist = (from player in PhotonNetwork.PlayerList orderby player.CustomProperties["damage"] descending select player).ToList();
        int i = 0;
        foreach (var player in sortedPlayerslist)
        {
            slots[i].SetActive(true); // Activate the slot for each player in the sorted list
            if (player.NickName == "")
            {
                nametexts[i].text = "unnamed"; // If the player's nickname is empty, set it to "unnamed"
            }

            nametexts[i].text = player.NickName; // Set the player's nickname in the UI
                                                 // Set the player's score in the UI

            if (player.CustomProperties["kills"] != null)
            {
                kdtexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"]; // Set the player's kills and deaths in the UI
                scoretexts[i].text = player.CustomProperties["damage"].ToString();
            }
            else
            {
                kdtexts[i].text = "0/0"; // If the player has no kills or deaths, set it to "0/0" 
            }

            i++;
            if (i >=3) return;
        }

    }
    void timer()
    {
        
        int minutes = Mathf.FloorToInt(duration / 60F);
        int seconds = Mathf.FloorToInt(duration%60);
        timertext.text = $"{minutes}:{seconds:00}";
    }

    public void LeaveTheRoom()
    {
        PhotonNetwork.LeaveRoom();
        if (photonView.IsMine) Time.timeScale = 1f; // Reset time scale when leaving the room
        duration = timerdur;
        timerstarted = false; // Reset the timer state
        //EndUI.SetActive(false); // Hide the end UI when leaving the room
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
    public override void OnLeftRoom()
    {
        
        // Optionally, you can add logic here to handle what happens when the player leaves the room
        Debug.Log("Left the room successfully.");
        SceneManager.LoadScene(0); // Reset the timer duration    
        // You might want to load a different scene or reset the game state here
    }
}