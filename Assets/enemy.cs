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


    private AudioSource fxdie;
    private AudioSource fxhit;

    [SerializeField] AudioClip DieClip;
    [SerializeField] AudioClip HitClip;

    // Start is called before the first frame update
    void Start()
    {
        fxdie = gameObject.AddComponent<AudioSource>();
        fxdie.clip = DieClip;
        fxhit= gameObject.AddComponent<AudioSource>();
        fxhit.clip = HitClip;

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
         fxhit.Play();
         Debug.Log("Hurt");
         fxdie.Pause();

        if(currentHealth <= 0) 
        {
            Die();
            fxdie.Play();
            fxhit.Pause();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        
       

     if(gameObject.name == "Boss") GameObject.Find("SceneManager").GetComponent<LevelFinalManager>().BossDefeated();
        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the enemy
        body.isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        //animator.enabled = false;
        this.enabled = false;
        
    }
}
