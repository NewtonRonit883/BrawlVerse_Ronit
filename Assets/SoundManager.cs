using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource soundsource;
    public AudioSource Click;
    public AudioClip clicksfx;
    public AudioClip[] SoundClips;
    private void Start()
    {
        PlaySound();
    }
    public void PlaySound()
    {
        soundsource.clip = SoundClips[0];
        
        soundsource.volume = 0.8f; // Set volume to a reasonable level
        soundsource.Play();
    }
    
    public void Click_sound()
    {
        Click.clip = clicksfx;
        Click.volume = 0.7f;
        Click.Play();
    }
    


}
