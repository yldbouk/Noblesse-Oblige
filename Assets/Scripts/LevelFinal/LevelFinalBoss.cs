using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinalBoss : MonoBehaviour
{
    GameObject player;
    SpriteRenderer sprite;
    Animator animator;
    Rigidbody2D rb;
    public AudioSource hurt;
    public AudioSource hurtDead;

    public Transform attack1point1;
    public Transform attack1point2;
    public Transform attack2point1;
    public Transform attack2point2;
    public LayerMask enemyLayer;

    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;
    
    private bool direction = true;

    public int maxHealth = 100;
    int currentHealth;
    public healthbarBehavior healthbar;

    public GameObject flyingSword;

    private List<int> attacks = new List<int> { 0, 1, 2 };
    private bool readyToAttack = true;
    private bool _attacking;
    public bool attacking {
        get {
        return _attacking;
        }
    }

    void HELPERBeginAttack() { _attacking = true; }
    void HELPEREndAttack()   { _attacking = false; }

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (currentHealth <= 0 || !readyToAttack) return;
        if(attacks.Count == 0) attacks = new List<int> {0,1,2};
        int attack = Random.Range(0, attacks.Count);
        switch (attacks[attack])
        {
            case 0: readyToAttack = false; StartCoroutine(AttackRush());      break;
            case 1: readyToAttack = false; StartCoroutine(AttackJumpRush());  break;
            case 2: readyToAttack = false; StartCoroutine(AttackJumpThrow()); break;

        }
        attacks.RemoveAt(attack);
    }

    private void DealDamage(bool attack1 = true)
    {
        var point1 = attack1 ? attack1point1 : attack2point1;
        var point2 = attack1 ? attack2point1 : attack2point1;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll((!sprite.flipX ? point1 : point2).position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies) enemy.GetComponent<Move>().TakeDamage(attackDamage);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0) Die();
        else hurt.Play();
    }

    void Die()
    {
        hurtDead.Play();
        if (animator.GetBool("Dead")) return;
        animator.SetBool("Dead", true);
        StartCoroutine(GameObject.Find("SceneManager").GetComponent<LevelFinalManager>().BossKilled());
        enabled = false;
    }

    private IEnumerator AttackRush()
    {
        animator.SetInteger("state", 1);

        while (Vector2.Distance(transform.position, player.transform.position) > 1.9f)
        {
            int direction = transform.position.x < player.transform.position.x ? 1 : -1;
            sprite.flipX  = transform.position.x > player.transform.position.x;
            transform.position = new Vector2(transform.position.x + direction * .1f, transform.position.y);
            yield return new WaitForSecondsRealtime(.01f);
        }
        animator.SetInteger("state", 0);
        animator.SetTrigger("attack");
        yield return new WaitForSecondsRealtime(.5f);
        DealDamage();
        yield return new WaitForSecondsRealtime(2);
        readyToAttack = true; 

    }

    private IEnumerator AttackJumpRush()
    {
        gameObject.layer = 12;
        animator.SetTrigger("jump");
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSecondsRealtime(1);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(2);
        
        animator.SetBool("falling", true);
        rb.gravityScale = 2;
        rb.velocity = player.transform.position - transform.position;
        sprite.flipX = transform.position.x > player.transform.position.x;
        yield return new WaitForSecondsRealtime(.1f);
        while (rb.velocity.y < 0) yield return null;
        gameObject.layer = 11;

        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        yield return new WaitForSecondsRealtime(.65f);
        DealDamage();
        rb.gravityScale = 1;
        yield return new WaitForSecondsRealtime(2);

        readyToAttack = true;
    }

    private IEnumerator AttackJumpThrow()
    {
        gameObject.layer = 12;
        animator.SetTrigger("jump");
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSecondsRealtime(1);
        
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        animator.SetBool("falling", true);

        for (int i = 0; i < 8; i++) { StartCoroutine(Throw()); yield return new WaitForSecondsRealtime(.3f); }
        yield return new WaitForSecondsRealtime(2);

        animator.SetBool("falling", true);
        rb.gravityScale = 2;
        rb.velocity = player.transform.position - transform.position;
        sprite.flipX = transform.position.x > player.transform.position.x;
        yield return new WaitForSecondsRealtime(.1f);
        while (rb.velocity.y < 0) yield return null;
        gameObject.layer = 11;
        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        yield return new WaitForSecondsRealtime(.65f);
        DealDamage(false);
        rb.gravityScale = 1;
        yield return new WaitForSecondsRealtime(2);
        readyToAttack = true;
    }

    private IEnumerator Throw()
    {
        Vector2 pos = new Vector2(Camera.main.transform.position.x + Random.Range(-10, 11), 10);
        var forsee = Instantiate(flyingSword, pos, Quaternion.AngleAxis(135, Vector3.forward)); 
        forsee.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(1);
 
        var sword = Instantiate(flyingSword, pos, Quaternion.AngleAxis(135, Vector3.forward));
        sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 60);
    }

    public IEnumerator GoTo(Vector2 w)
    {
        animator.SetInteger("state", 1);
        int direction = transform.position.x < w.x ? 1 : -1;
        sprite.flipX  = transform.position.x > w.x;
        do {
            transform.position = new Vector2(transform.position.x + direction*.1f, transform.position.y);
            yield return new WaitForSecondsRealtime(.01f);
        } while (Vector2.Distance(transform.position, w) > 1.9f);
        animator.SetInteger("state", 0);
    }

    void OnDrawGizmosSelected()
    {
        if ((attack1point1 == null && attack1point2 == null) || (attack2point1 == null && attack2point2 == null))
            return;

        if (!direction)
        {
            Gizmos.DrawWireSphere(attack1point1.position, attackRange);
            Gizmos.DrawWireSphere(attack2point1.position, attackRange);
        }
        else
        {
            Gizmos.DrawWireSphere(attack1point2.position, attackRange);
            Gizmos.DrawWireSphere(attack2point2.position, attackRange);
        }
    }

}
