using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = .5f;
    public Transform target;


    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 newPos = new Vector3(target.position.x, target.position.y, 1f);

        if (FollowSpeed * Time.time <= 3f)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
            transform.Translate(newPos);
        }
        Debug.Log(newPos);
    }
}
