using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    GameObject target;

    [SerializeField] float speed;
    [SerializeField] float distanceThreshold;
    [SerializeField] float attackCooldown;
    [SerializeField] bool invertAxis;

    private bool attacking = false;

    [SerializeField] Transform attackPoint1;
    [SerializeField] Transform attackPoint2;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int attackDamage = 40;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float attackRate = 2f;

    private bool direction;

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
            direction = (transform.position.x < target.transform.position.x);
            sprite.flipX = (transform.position.x > target.transform.position.x) ^ invertAxis;
            transform.position = new Vector2(transform.position.x + (direction ? 1 : -1) * speed /100, transform.position.y);
            return;
        }
        readyToAttack = false;
        StartCoroutine(Attack());

    }

    private IEnumerator Attack() 
    {
        // Play an attack animation
        animator.SetBool("isRunning", false);
        animator.SetBool("isNearPlayer", true);

        // Detect enemies in range of attack

        Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, enemyLayer);
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, enemyLayer);

        // Damage them
        foreach (Collider2D enemy in hitEnemies1)
        {
            enemy.GetComponent<enemy>().TakeDamage(attackDamage);
        }
        foreach (Collider2D enemy in hitEnemies2)
        {
            enemy.GetComponent<enemy>().TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(.25f);
        animator.SetBool("isNearPlayer", false);
        yield return new WaitForSeconds(attackCooldown);
        readyToAttack = true;
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null && attackPoint2 == null)
            return;

        if (!direction)
            Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
        else
            Gizmos.DrawWireSphere(attackPoint1.position, attackRange);
    }

}
