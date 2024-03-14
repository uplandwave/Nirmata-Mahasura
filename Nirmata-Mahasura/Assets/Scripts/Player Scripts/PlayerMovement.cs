using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    PhotonView view;

    public float runSpeed = 150f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public void Start()
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
        if (view.IsMine)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    //public void OnCrouching()
    //{
    //    animator.SetBool("IsCrouching", isCrouching);
    //}

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
