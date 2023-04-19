using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGKmovement : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float jumpPower = 5.0f;

    private Rigidbody2D playerRigidbody;
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
        if(playerRigidbody == null){
            Debug.Log("Player is missing a rigidbody component.");
        }
    }
    private void Update()
    {
        MovePlayer();
        if(Input.GetButton("Jump") && IsGrounded())
            Jump();
    }
    private void MovePlayer()
    {
        var hInput = Input.GetAxis("Horizontal");
        playerRigidbody.velocity = new Vector2(hInput * playerSpeed, playerRigidbody.velocity.y);
    }
    private void Jump()
    {
        var vInput = Input.GetAxis("Vertical");
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.y , vInput * jumpPower);
    } 

    private bool IsGrounded()
    {
        var groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f);
        return groundCheck.collider != null && groundCheck.collider.CompareTag("Ground");
    }
}

