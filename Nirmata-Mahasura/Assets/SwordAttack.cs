using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordAttack : MonoBehaviour
{
    public Animator animator;
    public PhotonView view;

    public Transform attackPoint;
    public float attackRange = .05f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;


    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log("We Hit " + enemy.name);
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}