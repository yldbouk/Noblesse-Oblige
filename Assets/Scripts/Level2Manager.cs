using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{

    MainManager mainManager; 


    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        mainManager.OverlayOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
