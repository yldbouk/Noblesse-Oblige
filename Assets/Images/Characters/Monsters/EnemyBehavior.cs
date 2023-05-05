using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyBehavior : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    GameObject target;

    [SerializeField] float speed;
    [SerializeField] float attackDistanceThreshold;
    [SerializeField] float trackDistanceThreshold;
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
        float distanceFromPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (distanceFromPlayer > trackDistanceThreshold) return;
        if (distanceFromPlayer > attackDistanceThreshold)
        {
            animator.SetBool("IsRunning", true);
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
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsNearPlayer", true);

        // Detect enemies in range of attack
        yield return new WaitForSecondsRealtime(.65f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(((!sprite.flipX ^ invertAxis) ? attackPoint1 : attackPoint2).position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies) enemy.GetComponent<Move>().TakeDamage(attackDamage);

        yield return new WaitForSecondsRealtime(.1f);
        animator.SetBool("IsNearPlayer", false);
        yield return new WaitForSecondsRealtime(attackCooldown);
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
