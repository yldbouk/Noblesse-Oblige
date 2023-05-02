using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

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

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) return;

        desiredDash |= input.RetrieveDodgeInput();
        
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        if(desiredDash && canDash)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
            Debug.Log(lastImageXpos);
            StartCoroutine(Dash());
            desiredDash = false;
        }
    }

    private IEnumerator Dash()
    {
        //initializing different variables needed for a good dash
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        //!direction == left, direction == right
        float direction = 1f;

        if(body.velocity.x < -0.1f)
            direction = -1f;
        else
            direction = 1f;
        
        //this vector2 is what actually moves the body forward
        body.velocity = new Vector2 ((transform.localScale.x * direction) * dashingPower, 0f);
        //Debug.Log(transform.localScale.x * direction);


        //this is for the afterimage for the knight
        if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
        {
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
        
    }


}
