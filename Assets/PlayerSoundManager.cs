using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("FootStep")]
    public AudioSource footstepSource;
    public AudioClip footstepSFX;
    [Space]
    [Header("Jump")]
    public AudioSource jumpSource;
    public AudioClip jumpSFX;
    [Space]
    [Header("Dash")]
    public AudioSource dashSource;
    public AudioClip dashSFX;
    [Space]
    [Header("Attack")]
    public AudioSource attackSource;
    public AudioClip[] attackSFX;

    public void PlayFootsteps()
    {
        GetComponent<PhotonView>().RPC("PlayFootstepsRPC", RpcTarget.All);
    }
    [PunRPC]
    public void PlayFootstepsRPC()
    {
        footstepSource.clip = footstepSFX;
        footstepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f); // Randomize pitch for more natural sound
        footstepSource.volume = UnityEngine.Random.Range(0.6f, 0.75f); // Set volume to a reasonable level
        footstepSource.Play();
    }
    public void PlayJump()
    {
        GetComponent<PhotonView>().RPC("PlayJumpRPC", RpcTarget.All);
    }
    [PunRPC]
    public void PlayJumpRPC()
    {
        jumpSource.clip = jumpSFX;
        jumpSource.volume = 0.7f; // Set volume to a reasonable level
        jumpSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Randomize pitch for more natural sound
        jumpSource.Play();
    }
    public void PlayAttack(int attackIndex)
    {
        GetComponent<PhotonView>().RPC("PlayAttackRPC", RpcTarget.All, attackIndex);
    }
    [PunRPC]
    public void PlayAttackRPC(int attackIndex)
    {
        attackSource.clip = attackSFX[attackIndex];
        attackSource.volume = 0.85f; // Set volume to a reasonable level
        attackSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Randomize pitch for more natural sound
        attackSource.Play();
    }
    public void PlayDash()
    {
        GetComponent<PhotonView>().RPC("PlayDashRPC", RpcTarget.All);
    }
    [PunRPC]
    public void PlayDashRPC()
    {
        dashSource.clip = dashSFX;
        dashSource.volume = 0.75f; // Set volume to a reasonable level
        dashSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Randomize pitch for more natural sound
        dashSource.Play();
    }
}
