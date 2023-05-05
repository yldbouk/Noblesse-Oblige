using Unity.VisualScripting;
using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    PolygonCollider2D c;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public LayerMask enemyLayer;

    void Start()
    {
        c = GetComponent<PolygonCollider2D>();

    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - .5f);
        if (transform.position.y < -10) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        c.enabled= false;
        collision.GetComponent<Move>().TakeDamage(attackDamage);

    }

}
