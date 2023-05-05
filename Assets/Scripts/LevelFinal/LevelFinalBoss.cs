using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditorInternal;
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
    bool readyToAttack = true;
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

    private void collideCheck()
    {
        Collider2D[] hit1Enemies1 = Physics2D.OverlapCircleAll(attack1point1.position, attackRange, enemyLayer);
        Collider2D[] hit1Enemies2 = Physics2D.OverlapCircleAll(attack1point2.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hit1Enemies1)
            enemy.GetComponent<Move>().TakeDamage(attackDamage);
        foreach (Collider2D enemy in hit1Enemies2)
            enemy.GetComponent<Move>().TakeDamage(attackDamage);
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

    //Attack 1
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
        yield return new WaitForSeconds(.5f);
        collideCheck();
        yield return new WaitForSeconds(2);
        readyToAttack = true; 

    }

    //Attack 2
    private IEnumerator AttackJumpRush()
    {
        animator.SetTrigger("jump");
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2);

        animator.SetBool("falling", true);
        rb.gravityScale = 2;
        rb.velocity = player.transform.position - transform.position;
        sprite.flipX = transform.position.x > player.transform.position.x;
        yield return new WaitForSeconds(.1f);
        while (rb.velocity.y < 0) yield return null;
        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        yield return new WaitForSeconds(.65f);
        collideCheck();
        rb.gravityScale = 1;
        yield return new WaitForSeconds(2);
        readyToAttack = true;
    }

    private IEnumerator AttackJumpThrow()
    {
        animator.SetTrigger("jump");
       // collideCheck();
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        animator.SetBool("falling", true);

        for (int i = 0; i < 8; i++) { StartCoroutine(Throw()); yield return new WaitForSeconds(.3f); }
        yield return new WaitForSeconds(2);

        animator.SetBool("falling", true);
        rb.gravityScale = 2;
        rb.velocity = player.transform.position - transform.position;
        sprite.flipX = transform.position.x > player.transform.position.x;
        yield return new WaitForSeconds(.1f);
        while (rb.velocity.y < 0) yield return null;
        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        yield return new WaitForSeconds(.65f);
        collideCheck();
        rb.gravityScale = 1;
        yield return new WaitForSeconds(2);
        readyToAttack = true;
    }

    private IEnumerator Throw()
    {
        int offset = Random.Range(-10, 11);
        var forsee = Instantiate(flyingSword, new Vector2(Camera.main.transform.position.x + offset, 10), Quaternion.AngleAxis(135, Vector3.forward)); 
        forsee.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(1);
 
        var sword = Instantiate(flyingSword, new Vector2(Camera.main.transform.position.x + offset, 10), Quaternion.AngleAxis(135, Vector3.forward));
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
            Gizmos.DrawWireSphere(attack1point2.position, attackRange);
            Gizmos.DrawWireSphere(attack2point2.position, attackRange);
        }
        else if (direction)
        {
            Gizmos.DrawWireSphere(attack1point1.position, attackRange);
            Gizmos.DrawWireSphere(attack2point1.position, attackRange);
        }
    }

}
