using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    private LevelFinalBoss boss = null;


    private new AudioSource audio;

    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip hitClip;


    // Start is called before the first frame update
    void Start()
    {
        if (tag == "Boss") { boss = GetComponent<LevelFinalBoss>(); return; }
        audio = gameObject.AddComponent<AudioSource>();
        currentHealth = maxHealth;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    
    }

    

    public void TakeDamage(int damage)
    {
        if (boss != null) {boss.TakeDamage(damage); return; }

        currentHealth -= damage;
        //Play hurt animation
        animator.SetTrigger("Hurt");
        audio.clip = hitClip; audio.Play();
        //Debug.Log("Hurt");

        if(currentHealth <= 0) 
        {
            Die();
            audio.clip = dieClip; audio.Play();
        }
    }

    void Die()
    {
        //Debug.Log("Enemy died");
        
        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the enemy
        gameObject.layer = 12;
        enabled = false;
        
    }
}
