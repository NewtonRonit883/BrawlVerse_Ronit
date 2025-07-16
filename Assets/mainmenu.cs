using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenuUI;
    public GameObject LobbyUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            mainMenuUI.SetActive(false); // Hide the main menu UI when Enter is pressed
            LobbyUI.SetActive(true); // Show the lobby UI
        }
        // KeyCode for Enter 
    }
}
