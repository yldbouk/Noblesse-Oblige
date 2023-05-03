using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelFinalPlayer : MonoBehaviour
{
    Vector2 waypoint = new Vector2(-5,-1.932f);
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public IEnumerator GoToWaypoint()
    {
        animator.SetInteger("state", 1);
        do {
            transform.position = new Vector2(transform.position.x + .06f, transform.position.y);
            yield return new WaitForSecondsRealtime(.01f);
        } while (Vector3.Distance(transform.position, waypoint) > .03f);
        animator.SetInteger("state", 0);
    }
}
