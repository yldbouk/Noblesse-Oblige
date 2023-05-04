using UnityEngine;

public class Jump : MonoBehaviour
{


    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 4f;
    [SerializeField, Range(0, 5)] private float maxAirjumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 3f;
    [SerializeField] public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;
    private enum animationState {idle, running, jumping, falling }
    animationState state;
    private Ground ground;
    private Vector2 velocity;
    MainManager mainManager;

    private int jumpPhase;
    private float defaultGravityScale;

    
    private bool desiredJump;
    public bool onGound;
    const float k_GroundedRadius = .2f;                         // Radius of the overlap circle to determine if grounded

    // Start is called before the first frame update
    void Awake()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainManager.inCutscene) return;

        onGound = false;
        desiredJump |= input.RetrieveJumpInput();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject != gameObject)
                onGound = true;

        if (velocity.y > 0.5 && !onGound)
        {
            state = animationState.jumping;
            animator.SetInteger("state", (int)state);
        }
        else if (velocity.y < -0.5 || !onGound)
        {
            state = animationState.falling;
            animator.SetInteger("state", (int)state);
        }
        
        
    }

    private void FixedUpdate()
    {
        if (mainManager.inCutscene) return;

        
        velocity = body.velocity;

        if (onGound)
            jumpPhase = 0;

        if (desiredJump)
        {
            Debug.Log(body.velocity.y);
            Debug.Log(onGound);
            desiredJump = false;
            
            JumpAction();
        }

        if (body.velocity.y > 0.01)
        {
            body.gravityScale = upwardMovementMultiplier;
            
        }
        else if (body.velocity.y < -0.01)
        {
            body.gravityScale = downwardMovementMultiplier;
            
        }
        else if (body.velocity.y == 0)
        { 
            body.gravityScale = defaultGravityScale;
            
        }
        body.velocity = velocity;
        
    }

    private void JumpAction()
    {
        if(onGound)
        {
            
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            if (velocity.y > 0f)
                jumpSpeed = Mathf.Max(jumpSpeed = velocity.y, 0f);

            velocity.y += jumpSpeed;
        }
    }
}
