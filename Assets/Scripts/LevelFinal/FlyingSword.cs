using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    Transform attack1point1;
    Transform attack1point2;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public LayerMask enemyLayer;

    void Start()
    {
        attack1point1 = GameObject.Find("attack1point1").transform;
        attack1point2 = GameObject.Find("attack1point2").transform;
        enemyLayer = 1 << 6;
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y-.2f);
        if(transform.position.y < -10) Destroy(gameObject);
        //Collider2D[] hit1Enemies1 = Physics2D.OverlapCircleAll(attack1point2.position, attackRange, enemyLayer);
        //foreach (Collider2D enemy in hit1Enemies1)
        //    enemy.GetComponent<enemy>().TakeDamage(attackDamage);
    }
}
