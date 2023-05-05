using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1BGMHelper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    { GameObject.Find("Manager").GetComponent<MainManager>().PlayBGM(GameObject.Find("SceneManager").GetComponent<Level1Manager>().backgroundMusic);  Destroy(gameObject); }
}
