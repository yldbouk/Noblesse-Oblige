using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public SpriteRenderer sprite;

    private Transform currentPoint;
    public float speed;

    public bool readyToAttack = false;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (!readyToAttack)
        {
            animator.SetBool("isRunning", true);
            int direction = transform.position.x < body.transform.position.x ? 1 : -1;
            sprite.flipX = transform.position.x > body.transform.position.x;
            transform.position = new Vector2(transform.position.x + direction * 0.1f, transform.position.y);
            readyToAttack = false;
            StartCoroutine(Attack());
        }

        readyToAttack = false;
    }

    private IEnumerator Attack() 
    {
        
        //while (Vector2.Distance(transform.position, body.transform.position) > 1.9f)
        
        animator.SetBool("isRunning", false);
        animator.SetBool("isNearPlayer", true);
        yield return new WaitForSeconds(5);
        readyToAttack = false;
    }

    
}
