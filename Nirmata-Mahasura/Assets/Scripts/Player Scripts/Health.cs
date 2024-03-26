using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100;
    float currentHealth;
    // Start is called before the first frame update
    [SerializeField] LinkedHealthBar healthBar;
    void Awake()
    {
        healthBar = GetComponentInChildren<LinkedHealthBar>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(healthBar, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        animator.SetTrigger("Hurt");


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    async void Die()
    {
        //Debug.Log("Enemy Died!");

        animator.SetBool("IsDead", true);

        //GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}