using System.Collections;
using UnityEngine;

public class Dodge : MonoBehaviour
{

    [SerializeField] private InputController input = null;               
    [SerializeField] public Transform m_GroundCheck;                        
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;
    private bool desiredDash;


    private bool canDash = true;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public float distanceBetweenImages;
    private float direction = 1f;

    MainManager mainManager;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();

    }

    void Update()
    {
        if (mainManager.inCutscene) return;
        desiredDash |= input.RetrieveDodgeInput();
    }

    private void FixedUpdate()
    {
        if (isDashing || mainManager.inCutscene) return;

        if(body.velocity.x <= -0.01f)
            direction = -1f;
        else if(body.velocity.x >= 0.01f)
            direction = 1f;


        if(desiredDash && canDash)
        {
            if (body.velocity.y <= 0.1f) animator.SetTrigger("roll");
            else if (body.velocity.y >= 0.2f) animator.SetTrigger("airDodge");
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
            Debug.Log(body.velocity);
            StartCoroutine(Dash());
            desiredDash = false;
        }
    }

    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(6, 11, true);
        //initializing different variables needed for a good dash
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        //!direction == left, direction == right
        
        
        Debug.Log(direction);
        //this vector2 is what actually moves the body forward
        body.velocity = new Vector2 ((transform.localScale.x * direction) * dashingPower, 0f);
        //Debug.Log(transform.localScale.x * direction);


        //this is for the afterimage for the knight
        if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
        {
            Debug.Log(PlayerAfterImagePool.Instance);
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }

        //stops player from dashing multible times in a row
        yield return new WaitForSeconds(dashingTime);

        //this is also for the afterimage
        

        //sets the gravity back to normal
        body.gravityScale = originalGravity;

        //takes it out of the dashing state
        isDashing = false;

        //a dash cooldown
        yield return new WaitForSeconds(dashingCooldown);

        //puts it in the "can dash" state, allowing it to be called and ran again
        canDash = true;
        Physics2D.IgnoreLayerCollision(6, 11, false);
    }


}
