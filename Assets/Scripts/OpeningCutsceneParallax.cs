using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OpeningCutsceneParallax : MonoBehaviour
{
    public float parallaxSpeed = .01f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Prologue Camera Script Loaded");
        StartCoroutine(UpdateParallax());
    }

    IEnumerator UpdateParallax()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(-parallaxSpeed, 0, 0);
        }
        
    }
}
