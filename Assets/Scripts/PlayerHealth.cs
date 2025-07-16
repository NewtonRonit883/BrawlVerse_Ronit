using CartoonFX;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public PlayerStateMachine _playerStateMachine;  // Reference to your existing shield script
    public bool isLocalPlayer;
    [Header("Damage VFX")]
    public GameObject hitVFX;
    public Transform VFXpoint;
    public GameObject deathVFX;
    [SerializeField] private float extra = 20f; // Offset for hit effect position
    [Header("Health UI")]
    public RectTransform healthbar; // Reference to the health UI GameObject
    public RectTransform Healthbarmain;
    private float originalHealthBarSize;
    [Space]
    public RectTransform healthbar3D; // Reference to the health UI GameObject for non-local players
    public RectTransform Healthbarmain3D;
    private float originalHealthBarSize3D;

    
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
        if (!isLocalPlayer)
        {
            Healthbarmain.gameObject.SetActive(false); // Hide health bar for non-local players
            Healthbarmain3D.gameObject.SetActive(true); // Show 3D health bar for non-local players 
        }
        originalHealthBarSize = healthbar.sizeDelta.x; // Store the original size of the health bar
        originalHealthBarSize3D = healthbar3D.sizeDelta.x; // Store the original size of the 3D health bar
        if (_playerStateMachine == null)
        {
            
            _playerStateMachine = GetComponent<PlayerStateMachine>(); // Try auto-assign if on same object
        }
        
    }
    
    [PunRPC]
    public void TakeDamage(Vector3 hitpoint,int damageAmount)
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
        
        hitVFX.GetComponent<CFXR_ParticleText>().text = damageAmount.ToString(); // Set the damage amount in the hit effect    
        GameObject score = PhotonNetwork.Instantiate(hitVFX.name, VFXpoint.position, Quaternion.identity); // Spawn hit effect at the hit point
        //Debug.Log("Player health: " + photonView.Owner.NickName + health);
        Destroy(score, 2);
        if (health<=0)
        {
            
            if (isLocalPlayer)
            {
                RoomManager.Instance.SpawnPlayer();
                RoomManager.Instance.deaths++;
                RoomManager.Instance.SetHashes();
                Debug.Log("Player died!");
                GameObject deathEffect = PhotonNetwork.Instantiate(deathVFX.name, VFXpoint.position, Quaternion.identity);
                Destroy(deathEffect, 2f); // Destroy the death effect after 2 seconds


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
        if (healthbar3D != null)
        {
            float currentHealth = Mathf.Clamp(health, 0f, 100f);
            healthbar3D.sizeDelta = new Vector2(originalHealthBarSize3D * (currentHealth / 100f), healthbar3D.sizeDelta.y);
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
