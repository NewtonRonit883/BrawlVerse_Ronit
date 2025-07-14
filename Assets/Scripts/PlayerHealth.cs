using System.Collections;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public PlayerStateMachine _playerStateMachine;  // Reference to your existing shield script
    public bool isLocalPlayer;

    [Header("Health UI")]
    public RectTransform healthbar; // Reference to the health UI GameObject
    private float originalHealthBarSize;
    
    void Start()
    {
       
        /*if (isLocalPlayer)
        {
            if (photonView.Owner.CustomProperties.TryGetValue("health", out object healthObj) && healthObj is int h)
            {
                health = h;
            }
            else
            {
                health = 100; // or your default value
            }
            // Initialize health from RoomManager
            
        }*/
        originalHealthBarSize = healthbar.sizeDelta.x; // Store the original size of the health bar
        if (_playerStateMachine == null)
        {
            
            _playerStateMachine = GetComponent<PlayerStateMachine>(); // Try auto-assign if on same object
        }
        
    }
    
    [PunRPC]
    public void TakeDamage(int damageAmount)
    {
        //if (!photonView.IsMine) return; // Only allow the local player to take damage   
        // If shield active, ignore damage
        if (_playerStateMachine != null && _playerStateMachine.isShieldActive)
        {
            Debug.Log("Shield is active! No damage taken.");
            return;
        }
        
        /*if (photonView.Owner.CustomProperties.TryGetValue("health", out object healthObj) && healthObj is int h)
        {
            health = h;
        }
        else
        {
            health = 100; // or your default value
        }*/
        health -= damageAmount;
        UpdateHealthUI(); // Update the health UI after taking damage

        //Debug.Log("Player health: " + photonView.Owner.NickName + health);

        if (health<=0)
        {
            
            if (isLocalPlayer)
            {
                RoomManager.Instance.SpawnPlayer();
                RoomManager.Instance.deaths++;
                RoomManager.Instance.SetHashes();
                Debug.Log("Player died!");

                
                //if (isLocalPlayer) RoomManager.Instance.SpawnPlayer();
                
            }
            Destroy(gameObject);
        }
    }
    private void UpdateHealthUI()
    {
        // Ensure healthbar is assigned and valid
        if (healthbar != null)
        {
            // Clamp health to prevent negative values or values above 100
            float currentHealth = Mathf.Clamp(health, 0f, 100f);
            healthbar.sizeDelta = new Vector2(originalHealthBarSize * (currentHealth / 100f), healthbar.sizeDelta.y);
            //Debug.Log("HealthBar UI updated for " + photonView.Owner.NickName + ": " + currentHealth);
        }
        else
        {
            Debug.LogWarning("Healthbar RectTransform is not assigned in the Inspector for " + gameObject.name);
        }
    }
    /*void Die()
    {
        if (photonView.IsMine)
        {
            //_animator.SetTrigger("Death");
            RespawnAfterDelay(1f); // Adjust the delay as needed
            
            
        }
    }

    public void RespawnAfterDelay(float delay)
    {
        if (gameObject != null) PhotonNetwork.Destroy(gameObject);
        
    }*/
}
