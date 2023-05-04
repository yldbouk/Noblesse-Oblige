using UnityEngine;

public class HoleCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        collision.transform.position = GameObject.FindGameObjectWithTag("Checkpoint").transform.position;
    }
}
