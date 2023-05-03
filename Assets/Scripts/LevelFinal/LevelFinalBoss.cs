using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LevelFinalBoss : MonoBehaviour
{
    GameObject player;
    SpriteRenderer sprite;
    Animator animator;
    Rigidbody2D rb;

    public GameObject flyingSword;

    

    bool readyToAttack = true;

    public int health = 10;

    private bool _attacking;
    public bool attacking {
        get {
        return _attacking;
        }
    }

    void HELPERBeginAttack() { _attacking = true; }
    void HELPEREndAttack()   { _attacking = false; }


    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        while (health > 0)
        {
            if (!readyToAttack) return;
            switch (Random.Range(0, 3))
            {
                case 0: readyToAttack = false; StartCoroutine(AttackRush()); break;
                case 1: readyToAttack = false; StartCoroutine(AttackJumpRush());  break;
                case 2: readyToAttack = false; StartCoroutine(AttackJumpThrow()); break;

            }
        }
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
        yield return new WaitForSeconds(5);
        readyToAttack = true; 

    }

    private IEnumerator AttackJumpRush()
    {
        animator.SetTrigger("jump");
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 0;
        yield return new WaitForSeconds(2);

        animator.SetBool("falling", true);
        rb.gravityScale = 2;
        rb.velocity = player.transform.position - transform.position;
        //yield return new WaitForSeconds(.1f);
        while (rb.velocity.y < 0) yield return null;
        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        rb.gravityScale = 1;
        yield return new WaitForSeconds(5);
        readyToAttack = true;
    }

    private IEnumerator AttackJumpThrow()
    {
        animator.SetTrigger("jump");
        rb.velocity = new Vector2(0, 15);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 0;
        yield return new WaitForSeconds(2);

        animator.SetBool("falling", true);

        int[] offsets = new int[4];
       
        for (int i = 0; i < 4; i++)
        {
            offsets[i] = Random.Range(-10, 11);
            Instantiate(flyingSword, new Vector2(Camera.main.transform.position.x + offsets[i], 10), Quaternion.AngleAxis(135, Vector3.forward));
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1);
        GameObject sword;
        for (int i = 0; i < 4; i++)
        {
            sword = Instantiate(flyingSword, new Vector2(Camera.main.transform.position.x + offsets[i], 10), Quaternion.AngleAxis(135, Vector3.forward));
            sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 60);

            yield return new WaitForSeconds(.5f);
        }

        rb.gravityScale = 1;
        while (rb.velocity.y < 0) yield return null;
        animator.SetBool("falling", false);
        animator.SetTrigger("attack2");
        yield return new WaitForSeconds(5);
        readyToAttack = true;
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

}
