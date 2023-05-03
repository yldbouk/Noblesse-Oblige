using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    GameObject target;

    public float speed;
    public float distanceThreshold;
    public float attackCooldown;

    bool readyToAttack = true;


    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (animator.GetBool("IsDead")) Destroy(this);
        if (!readyToAttack) return;

        if (Vector2.Distance(transform.position, target.transform.position) > distanceThreshold)
        {
            animator.SetBool("isRunning", true);
            int direction = (transform.position.x < target.transform.position.x ? 1 : -1);
            sprite.flipX = transform.position.x > target.transform.position.x;
            transform.position = new Vector2(transform.position.x + direction * speed /100, transform.position.y);
            return;
        }
        readyToAttack = false;
        StartCoroutine(Attack());

    }

    private IEnumerator Attack() 
    {  
        animator.SetBool("isRunning", false);
        animator.SetBool("isNearPlayer", true);
        yield return new WaitForSeconds(.25f);
        animator.SetBool("isNearPlayer", false);
        yield return new WaitForSeconds(attackCooldown);
        readyToAttack = true;
    }

    
}
