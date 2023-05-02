using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1Player : MonoBehaviour
{
    Vector2 waypoint = new Vector2(-5,-2);
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public IEnumerator GoToWaypoint()
    {
        animator.SetInteger("state", 1);
        do {
            transform.position = new Vector2(transform.position.x + .015f, transform.position.y);
            yield return null;
        }  while (Vector3.Distance(transform.position, waypoint) > .02f);
        animator.SetInteger("state", 0);
    }
}
