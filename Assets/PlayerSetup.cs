using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;
    private GameObject camera; // Reference to the player camera

    private void Awake()
    {
        // Ensure the PlayerStateMachine is assigned
        if (_playerStateMachine == null)
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
        }
        
        // Ensure the camera is assigned
        if (camera == null)
        {
            camera = transform.GetChild(4).gameObject;
        }
    }

    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            _playerStateMachine.enabled = true;
            camera.SetActive(true);
        }
        else
        {
            _playerStateMachine.enabled = false;
            camera.SetActive(false);
        }
    }
    public void IsLocalPlayer()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            _playerStateMachine.enabled = true;
            camera.SetActive(true);
        }
        else
        {
            _playerStateMachine.enabled = false;
            camera.SetActive(false);
        }

    }
    

}
