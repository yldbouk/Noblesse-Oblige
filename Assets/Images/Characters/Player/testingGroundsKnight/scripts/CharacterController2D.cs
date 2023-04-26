using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Header("Refs")]
    [Space]
    public float m_JumpForce = 400f;
    [Range(0, 1)]public float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
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
    // Start is called before the first frame update
    void Start()
    {
        var m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

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
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
