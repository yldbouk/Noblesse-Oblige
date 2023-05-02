using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float rollDistance = 4f;
    [SerializeField, Range(0, 5)] private float maxRoll = 0;
    [SerializeField, Range(0f, 5f)] private float rollMovementMultiplier = 3f;
    [SerializeField] public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;
    private Ground ground;

    private Vector2 direction;
    private Vector2 velocity;

    private bool desiredRoll;
    private bool onGound;
    const float k_GroundedRadius = .2f;                         // Radius of the overlap circle to determine if grounded


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        desiredRoll |= input.RetrieveDodgeInput();


    }
}
