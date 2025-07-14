using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using UnityEngine;
using Photon.Pun.Demo.PunBasics;
using static UnityEngine.UI.GridLayoutGroup;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerAttackState : PlayerBaseState
{
    private AttackData data;

    public PlayerAttackState(PlayerStateMachine ctx, PlayerStateFactory factory, AttackData attack)
        : base(ctx, factory)
    {
        data = attack;
    }

    public override void EnterState()
    {
        
        ctx.runtimeOverride["AttackBase"] = data.animation;
        ctx.animator.SetTrigger("isAttacking");
        ctx.animator.SetBool("IsAttacking", true);
    }

    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {
        ctx.animator.SetBool("IsAttacking", false);
    }
    public void ApplyDamage()
    {
        if (!ctx.attackOriginMap.TryGetValue(data.AttackOriginName, out var origin))
        {
            Debug.LogWarning($"No attack origin found for '{data.AttackOriginName}'. Using player transform as fallback.");
            origin = ctx.transform;
        }

        Collider[] hits = Physics.OverlapSphere(origin.position, data.range, ctx.EnemyLayer);
        foreach (var hit in hits)
        {
            /*---------------------------------------------------------------------*/
            if (hit.gameObject == ctx.gameObject)
            {

                Debug.Log("Skipped self hit");
                continue; // Skips rest of loop body moves to next hit
            }

            AttackEvents.Broadcast(ctx.gameObject, hit.gameObject, data);
            if (hit.TryGetComponent<PhotonView>(out var pv))
            {

                string playerName = pv.Owner.NickName;
                Debug.Log($"Hit player : {playerName} by {ctx.GetComponent<PhotonView>().Owner.NickName}");
                if (playerName == ctx.GetComponent<PhotonView>().Owner.NickName)
                {
                    Debug.Log("Skipped self hit");
                    continue; // Skips rest of loop body moves to next hit
                }
                
            }
            if (hit.TryGetComponent<PlayerStateMachine>(out var psm))
            {

                if (psm.wasParried)
                {
                    Debug.Log("Skipped due to parry");
                    continue; // Skips rest of loop body moves to next hit
                }
            }

            /*---------------------------------------------------------------------*/
           

            // Apply damage
            if (hit.TryGetComponent<PlayerHealth>(out var health))
            {
                //Debug.Log($"Applying damage to player with nickname: {health.photonView.Owner.NickName}");
                RoomManager.Instance.damage += data.damage;
                RoomManager.Instance.SetHashes();
                if (data.damage >= health.health)
                {
                    RoomManager.Instance.kills++;
                    RoomManager.Instance.SetHashes();

                }
                health.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, data.damage);
                
                



            }
            /*if (hit.TryGetComponent<PhotonView>(out var pv1))
            {
                RoomManager.Instance.damage += data.damage;
                RoomManager.Instance.SetHashes();
                int targetHealth = (pv1.Owner.CustomProperties.TryGetValue("health", out var h1) && h1 is int hInt) ? hInt : 100;
                if (data.damage >= targetHealth)
                {
                    RoomManager.Instance.kills++;
                    RoomManager.Instance.SetHashes();
                }
                int kills = pv1.Owner.CustomProperties.TryGetValue("kills", out var k) ? (int)k : 0;
                int deaths = pv1.Owner.CustomProperties.TryGetValue("deaths", out var d) ? (int)d : 0;
                int damage = pv1.Owner.CustomProperties.TryGetValue("damage", out var dmg) ? (int)dmg : 0;
                int health1 = pv1.Owner.CustomProperties.TryGetValue("health", out var h) ? (int)h : 100;

                // Modify these values as needed, e.g.:
                health1 -= data.damage;
                SetHash(pv.Owner, kills, deaths, damage, health1);


            }*/

           
            // Apply force if it has Rigidbody

            if (hit.attachedRigidbody != null)
            {
                Vector3 pushDir = (hit.transform.position - origin.position).normalized;
                hit.attachedRigidbody.AddForce(pushDir * data.pushForce, ForceMode.Impulse);
            }
        }
    }
    private void SetHash(Photon.Realtime.Player targetPlayer,int kills,int deaths,int damage,int health)
    {
        try
        {
            Hashtable hash = targetPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            hash["damage"] = damage;
            hash["health"] = health;
            targetPlayer.SetCustomProperties(hash);
        }
        catch
        {
            // Handle exceptions as needed
        }
    }

}
