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


    private new AudioSource audio;

    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip hitClip;
   

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
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
        audio.clip = hitClip; audio.Play();
         Debug.Log("Hurt");

        if(currentHealth <= 0) 
        {
            Die();
            audio.clip = dieClip; audio.Play();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        
     if(gameObject.name == "Boss") GameObject.Find("SceneManager").GetComponent<LevelFinalManager>().BossDefeated();
        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the enemy
        gameObject.layer = 12;
        enabled = false;
        
    }
}
