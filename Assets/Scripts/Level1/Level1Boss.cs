using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Boss : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void AttackHelper() { readyToAttack = true; }
    //public void Attack() { animator.SetBool("attack", true); animator.SetBool("attack", false); }
    public int animationState { set { animator.SetInteger("state", value); } }

    public bool readyToAttack = false;

    public IEnumerator GoTo(Vector2 w)
    {
        animator.SetInteger("state", 1);
        int direction = transform.position.x < w.x  ? 1 : -1;
  
        do
        {
            transform.position = new Vector2(transform.position.x + direction*.02f, transform.position.y);
            yield return new WaitForSecondsRealtime(.01f);
        } while (Vector2.Distance(transform.position, w) > 1.9f);
        animator.SetInteger("state", 0);
    }

}
