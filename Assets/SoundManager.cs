using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource soundsource;
    public AudioSource Click;
    public AudioSource JoinSource;
    public AudioClip JoinClip;
    public AudioClip clicksfx;
    public AudioClip[] SoundClips;
    public SoundController soundController;
    private void Start()
    {
        PlaySound(0);
    }
    public void PlaySound(int index)
    {
        soundsource.clip = SoundClips[index];
        if (soundController.isMuted)
        {
            soundsource.volume = 0f; // Mute the sound if muted
        }
        else
        {
            soundsource.volume = 0.45f; // Set volume to a reasonable level
        }
        //soundsource.volume = 0.45f; // Set volume to a reasonable level
        soundsource.Play();
    }
   
    public void Click_sound()
    {
        Click.clip = clicksfx;
        Click.volume = 0.7f;
        Click.Play();
    }
    public void JoinSound()
    {
        GetComponent<PhotonView>().RPC("PlayJoinRPC", RpcTarget.All);
    }
    [PunRPC]
    public void PlayJoinRPC()
    {
        JoinSource.clip = JoinClip;
        JoinSource.volume = 0.7f; // Set volume to a reasonable level
        JoinSource.Play();
    }

}
