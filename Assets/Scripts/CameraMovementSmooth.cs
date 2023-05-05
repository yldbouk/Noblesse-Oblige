using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementSmooth : MonoBehaviour
{
    [SerializeField] float followSharpness = 0.1f;
    private GameObject player;

    private void Start() { gameObject.GetComponent<CameraMovement>().enabled = false; player = GameObject.FindGameObjectWithTag("Player"); }

    void FixedUpdate()
    {
        if (player != null)
        {
            float blend = 1f - Mathf.Pow(1f - followSharpness, Time.deltaTime * 30f);
            Vector3 pos = Vector3.Lerp(transform.position, player.transform.position, blend);
            transform.position = new Vector3(pos.x, pos.y + .1f, -10);
        }
        else Destroy(this);
    }
}

