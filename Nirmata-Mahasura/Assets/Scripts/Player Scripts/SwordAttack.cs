using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor.PackageManager;

public class SwordAttack : MonoBehaviourPunCallbacks
{
    public Animator animator;
    public PhotonView view;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine && Input.GetButtonDown("Fire1"))
        {
            // Trigger attack animation locally
            animator.SetTrigger("Attack");

            // Call RPC to perform attack on all clients
            photonView.RPC("PerformAttack", RpcTarget.All);
        }
        else if (!PhotonNetwork.IsConnected && Input.GetButtonDown("Fire1")) // Check if offline
        {
            // Trigger attack animation locally
            animator.SetTrigger("Attack");

            // Perform attack offline
            PerformAttack();
        }
    }

    [PunRPC]
    void PerformAttack()
    {
        // Perform attack on all clients
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if the object has a PhotonView attached
            Health hitEnemy = enemy.GetComponent<Health>();
            if (hitEnemy != null)
            {
                hitEnemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}