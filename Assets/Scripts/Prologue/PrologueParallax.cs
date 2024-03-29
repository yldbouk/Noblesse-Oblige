using System.Collections;
using UnityEngine;

public class PrologueParallax : MonoBehaviour
{
    public float parallaxSpeed = .15f;
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
