using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : MonoBehaviour
{

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
