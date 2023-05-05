using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    MainManager mainManager;

    private void Start() {  mainManager = GameObject.Find("Manager").GetComponent<MainManager>(); }

    void Update()
    {
        GameObject objectToFollow = GameObject.FindGameObjectWithTag("Player");
        if (objectToFollow == null) return;
        GameObject[] poses = GameObject.FindGameObjectsWithTag("Camera Positions");
        if (poses.Length == 0) return;

        GameObject closestTarget = null;
        float closestDistance = 99999;
        foreach (GameObject pos in poses)
        {
            float distance = Vector2.Distance(objectToFollow.transform.position, pos.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = pos;
                closestDistance = distance;
            }
        }
        transform.position = new Vector3(closestTarget.transform.position.x, closestTarget.transform.position.y, -10);
    }
} 
