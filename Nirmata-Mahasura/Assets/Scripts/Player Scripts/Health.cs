using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animator;
    public int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        // If this is not our own player, disable the animator to prevent visual updates
        if (!photonView.IsMine && animator != null)
        {
            animator.enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
            return;

        currentHealth -= damage;

        animator?.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!photonView.IsMine)
            return;

        animator?.SetBool("IsDead", true);
        photonView.RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player and need to send our actual position to the network
            stream.SendNext(currentHealth);
        }
        else
        {
            // Network player, receive data
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}