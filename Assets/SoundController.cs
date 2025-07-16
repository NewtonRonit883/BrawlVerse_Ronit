using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cross;
    public bool isMuted;
    [HideInInspector]
    public SoundManager soundManager;
    private void Awake()
    {
        isMuted = false; // Initialize isMuted to false
    }
    void Start()
    {
        cross.SetActive(false);
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    public void Mutebutton()
    {
        if (isMuted)
        {
            isMuted = false;
            cross.SetActive(false);
            soundManager.soundsource.volume = 0.45f;
        }
        else
        {
            isMuted = true;
            cross.SetActive(true);
            soundManager.soundsource.volume = 0f; // Mute the sound
        }
    }
}
