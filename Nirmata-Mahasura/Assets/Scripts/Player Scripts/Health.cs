using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}