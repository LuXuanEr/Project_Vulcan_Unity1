using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed,jumpHigh,runSpeed;
    private float xInput;
    
    private int FacingDir = 1;
    private bool facingRight = true;
    private bool isGrounded;
    private int Jumpcounter = 0;
    
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        Movement();
        
        CheckInput();

        isGrounded = Physics2D.Raycast(transform.position,Vector2.down,groundCheckDistance,whatIsGround);
        
        Debug.Log(isGrounded);
        
        FlipController();

        AnimatorController();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            Jump();
            Jumpcounter -= 1;
        }else if (isGrounded)
        {
            Jumpcounter = 1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            runSpeed = 2;
        }
        else
        {
            runSpeed = 1;
        }


    }

    private void Movement()
    {
       
        rb.velocity = new Vector2(xInput * moveSpeed* runSpeed, rb.velocity.y );
        
        
    }

    private void Jump()
    {   
        if(isGrounded || Jumpcounter > 0)
        rb.velocity = new Vector2(xInput * moveSpeed, jumpHigh );
    }

    private void AnimatorController()
    {
        
        bool IsMoving = rb.velocity.x != 0;

        bool IsRunning = rb.velocity.x != 0 && Input.GetKey(KeyCode.LeftShift);
        
        anim.SetBool("IsRunning",IsRunning);
        
        anim.SetBool("IsMoving", IsMoving);
        
    }

    private void Flip()
    {
        FacingDir = FacingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
