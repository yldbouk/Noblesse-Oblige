using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class movement : MonoBehaviour
{

    [Header("Movement Settings")]
    [Space]

    public float speed = 10f;
    public float m_JumpForce = 400f;

    public float m_RollSpeed = 2.0f;
    public float dashDuration = 0f;

    private float inputX;
    private float inputY;
    private bool decent = false;

    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_Sprite;
    public Animator m_Animator;
    private enum m_AnimationState { idle, running, jumping, falling };
    m_AnimationState state;





    [Range(0, 1)] public float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] public float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField] public bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] public Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] public Collider2D m_CrouchDisableCollider;


    const float k_GroundedRadius = .2f;                         // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;
    private bool m_Rolling;                          // For determining which way the player is currently facing.

    [Space]
    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public class BoolEvent : UnityEvent<bool> { };
    public BoolEvent OnCrouchEvent;



    private void Start()
    {
        //calls the rigidbody component for movement
        m_Rigidbody = GetComponent<Rigidbody2D>();

        //calls the sprite renderer so the sprite can flip depending on which way it is facing
        m_Sprite = GetComponent<SpriteRenderer>();

        //calls the animator component to check the conditions of the different transitions between animation states
        m_Animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        //determines if the transition between running (left, right, or standing still) is true or false
        UpdateAnimationState();

        Vector3 MovementX = new Vector3(speed * inputX, 0.0f, 0.0f);
        //Vector3 MovementY = new Vector3(0.0f, m_JumpForce * inputY, 0.0f);
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings
        // .
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
 

        if ((m_Grounded == true) && (Input.GetButtonDown("Jump")))
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);
            UpdateAnimationState();
        }
        transform.Translate(MovementX * Time.deltaTime);

    }

    private void UpdateAnimationState()
    {
        //determines if the transition between running (left, right, or standing still) is true or false

        switch (inputX)
        {
            case -1:
                m_Sprite.flipX = true;
                runningToJump();
                break;

            case 1:
                m_Sprite.flipX = false;
                runningToJump();
                break;

            default:
                idleToJump();
                if (Input.GetKeyDown(KeyCode.LeftShift))
                roll();
                break;
        }
    }

    void runningToJump()
    {
        state = m_AnimationState.running;
        if (m_Grounded) state = m_AnimationState.running;
        if (Time.time >= dashDuration)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                roll();
                dashDuration = Time.deltaTime + 1.0f / 2.0f;
            }
        if (m_Rigidbody.velocity.y > 0.0f && m_Grounded) state = m_AnimationState.jumping;
        if ((m_Rigidbody.velocity.y <= 0.0f && !m_Grounded) || (m_Rigidbody.velocity.y >= 0.0f && !m_Grounded)) state = m_AnimationState.falling;
        m_Animator.SetInteger("state", (int)state);        
    }

    void idleToJump()
    {
        state = m_AnimationState.idle;
        if (m_Grounded) state = m_AnimationState.idle;
        if (m_Rigidbody.velocity.y > 0.1f && m_Grounded) state = m_AnimationState.jumping;
        else state = m_AnimationState.idle;
        if (m_Rigidbody.velocity.y <= 0.0f && !m_Grounded) state = m_AnimationState.falling;
        m_Animator.SetInteger("state", (int)state);
    }

    void roll()
    {
        state = m_AnimationState.idle;
        // Play an attack animation
        if (m_Grounded == true && m_Rolling == false){
            m_Rigidbody.velocity = new Vector2(0.0f, 0.0f);
            m_Animator.SetTrigger("roll");
            m_Rolling = true;
            dash();
        }
        m_Rolling = false;
        
    }

    void dash()
    {
        m_Rigidbody.velocity = new Vector2(m_RollSpeed * inputX, 0.0f);
        transform.Translate(((m_Rigidbody.velocity + m_Rigidbody.velocity) * 5.0f * Time.deltaTime));
        Debug.Log(m_Rigidbody.velocity * 5.0f);
    }
}
