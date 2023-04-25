using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class movement : MonoBehaviour
{

    //public Vector2 speed = new Vector2(50, 50);

    [Header("Movement Settings")]
    [Space]

    public float speed = 10f;
    public float m_JumpForce = 400f;
    [Range(0, 1)] public float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] public float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField] public bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] public Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] public Collider2D m_CrouchDisableCollider;

    const float k_GroundedRadius = .2f;                         // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;
    private bool m_FacingRight = true;                          // For determining which way the player is currently facing.

    [Space]
    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public class BoolEvent : UnityEvent<bool> { };

    public BoolEvent OnCrouchEvent;
    // Update is called once per frame
    void Update()
    {
        

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        

        Vector3 MovementX = new Vector3(speed * inputX, 0.0f, 0.0f);
        Vector3 MovementY = new Vector3(0.0f, m_JumpForce * inputY, 0.0f);
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        bool decent = true;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
             
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if ((m_Grounded == false) && (inputY != m_JumpForce))
        {
            //Debug.Log(decent);
        }
     

        if((m_Grounded == true) && (inputY > 0))
        {   
            transform.Translate(MovementY * Time.fixedDeltaTime);
            Debug.Log(m_Grounded);
        }
        
        transform.Translate(MovementX * Time.deltaTime);
        
    }

}
