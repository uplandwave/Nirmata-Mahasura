using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    PhotonView view;

    public float runSpeed = 10f;

    float horizontalMove = 150f;
    bool jump = false;
    bool crouch = false;

    void Start () 
    {
        view = GetComponent<PhotonView>();
    }
    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                animator.SetBool("IsCrouching", true);
            }
            else
            {
                crouch = false;
                animator.SetBool("IsCrouching", false);
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    //public void OnCrouching()
    //{
    //    animator.SetBool("IsCrouching", isCrouching);
    //}

    void FixedUpdate()
    {

        // Move our character
        if (view.IsMine)
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;

            // Synchronize movement over the network
            view.RPC("SyncMovement", RpcTarget.Others, transform.position, horizontalMove, jump);
        }
    }
    [PunRPC]
    void SyncMovement(Vector3 newPosition, float newHorizontalMove, bool newJump)
    {
        // Update the position, horizontal move, and jump of the player on other clients
        transform.position = newPosition;
        horizontalMove = newHorizontalMove;
        jump = newJump;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(horizontalMove);
            stream.SendNext(jump);
        }
        else
        {
            Vector3 newPosition = (Vector3)stream.ReceiveNext();
            float newHorizontalMove = (float)stream.ReceiveNext();
            bool newJump = (bool)stream.ReceiveNext();

            // Update the position, horizontal move, and jump of the player on the local client
            transform.position = newPosition;
            horizontalMove = newHorizontalMove;
            jump = newJump;
        }
    }
}
