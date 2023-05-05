using UnityEngine;

public class HoleCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        collision.GetComponent<Move>().TakeDamage(10);

        collision.attachedRigidbody.velocity = Vector3.zero;
        collision.transform.position = GameObject.FindGameObjectWithTag("Checkpoint").transform.position;
    }
}
