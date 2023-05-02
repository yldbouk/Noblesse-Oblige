using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Boss : MonoBehaviour
{
    Vector2 waypoint = new Vector2(-3.5f, -1.45f);
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void AttackHelper() { readyToAttack  = true; }
    public void Attack() { animator.SetTrigger("attack"); }
    public int animationState { set { animator.SetInteger("state", value); } }

    public bool readyToAttack = false;

    public IEnumerator GoToWaypoint()
    {
        animator.SetInteger("state", 1);
        do
        {
            transform.position = new Vector2(transform.position.x - .005f, transform.position.y);
            yield return null;
        } while (Vector3.Distance(transform.position, waypoint) > .1f);
        animator.SetInteger("state", 0);
    }

}
