using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public healthbarBehavior healthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthbar.SetHealth(currentHealth, maxHealth);
        
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Play hurt animation
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the enemy
        body.isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        //animator.enabled = false;
        this.enabled = false;
        
    }
}
