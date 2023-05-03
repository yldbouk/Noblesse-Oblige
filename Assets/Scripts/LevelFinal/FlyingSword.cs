using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y-.2f);
        if(transform.position.y < -10) Destroy(gameObject);
    }
}
