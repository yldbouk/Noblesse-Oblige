using UnityEngine;

public class LightAttack : MonoBehaviour
{

    [SerializeField] private InputController input = null;

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;

    private bool desireAttack;
    private bool attacking = false;

    public Transform attackPoint1;
    public Transform attackPoint2;
    public LayerMask enemyLayer;

    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;
    private bool direction = true;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (attacking) return;
        desireAttack |= input.RetrieveAttackInput();
    }

    void FixedUpdate()
    {
        if(attacking) return;
        
        if (body.velocity.x <= -0.01f)
            direction = false;
        else if (body.velocity.x >= 0.01f)
            direction = true;
        

        if (Time.time >= nextAttackTime)
        {
            if (desireAttack)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                desireAttack = false;
            }
        }
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

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
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null && attackPoint2 == null)
            return;

        if(!direction)
            Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
        else if(direction)
            Gizmos.DrawWireSphere(attackPoint1.position, attackRange);
    }
}
