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
    public void TriggerPunchSFX()
    {
        soundManager.PlayAttack(0);
    }
    public void TriggerKickSFX()
    {
        soundManager.PlayAttack(1);
    }
    public void TriggerStompSFX()
    {
        soundManager.PlayAttack(2);
    }
}
