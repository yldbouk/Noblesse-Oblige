using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    public Transform attack1point1;
    public Transform attack1point2;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public LayerMask enemyLayer;

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y-.2f);
        if(transform.position.y < -10) Destroy(gameObject);
        Collider2D[] hit1Enemies1 = Physics2D.OverlapCircleAll(attack1point2.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hit1Enemies1)
            enemy.GetComponent<enemy>().TakeDamage(attackDamage);
    }
}
