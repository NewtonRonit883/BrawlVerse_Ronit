using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEventToSFXManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerSoundManager soundManager;
    public void TriggerFootstepSFX()
    {
        soundManager.PlayFootsteps();
    }
    public void TriggerJumpSFX()
    {
        soundManager.PlayJump();
    }
}
