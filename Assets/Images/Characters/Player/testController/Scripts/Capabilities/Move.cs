using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField] Animator animator;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 4f;

    [Space]
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [Space]
    [SerializeField] AudioSource audio;
    private AudioClip dieClip;
    private AudioClip hurtClip;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    
    private enum animationState { idle, running, jumping, falling }
    animationState state;
    private Ground ground;
    MainManager mainManager;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    // Start is called before the first frame update
    void Awake()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }


    public void HelperCutsceneRoll() { if (mainManager.inCutscene) sprite.flipX = true; }

    // Update is called once per frame
    void Update()
    {
        if (mainManager.inCutscene) return;

        direction.x = input.RetrieveMoveInput();

        if(direction.x < 0f)
            sprite.flipX = true;
        else if(direction.x > 0f)
            sprite.flipX = false;

        if (velocity.x == 0f && velocity.y == 0f)
        {
            state = animationState.idle;
            animator.SetInteger("state", (int)state);
        }
        else if (velocity.x != 0f && onGround)
        {
            state = animationState.running;
            animator.SetInteger("state", (int)state);
        }


        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        if (mainManager.inCutscene) return;

        onGround = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;


        //Play hurt animation
        animator.SetTrigger("Hurt");
        
        Debug.Log("Hurt");

        if (currentHealth <= 0)
        {
            Die();
            
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        if (gameObject.name == "Boss") GameObject.Find("SceneManager").GetComponent<LevelFinalManager>().BossDefeated();
        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the enemy
        gameObject.layer = 12;
        enabled = false;

    }
}
