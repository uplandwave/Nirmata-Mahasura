using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] public HealthBar healthBar;
    public PhotonView photonView;
    public void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        photonView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine) // Only initialize health bar for the local player
        {
            currentHealth = maxHealth;
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            StartCoroutine(SendHealthUpdates());
        }
    }
    void Die()
    {
        //Debug.Log("Enemy Died!");

        animator.SetBool("IsDead", true);

        //GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
    IEnumerator SendHealthUpdates()
    {
        while (true)
        {
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, currentHealth);
            yield return new WaitForSeconds(2f); // Adjust the interval based on your preference
        }
    }

    [PunRPC]
    void UpdateHealthRPC(int health)
    {
        currentHealth = health;
        if (currentHealth > 0)
        {   
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        else
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        animator.SetTrigger("Hurt");


        if (currentHealth <= 0)
        {
            Die();
        }
    }
   

}