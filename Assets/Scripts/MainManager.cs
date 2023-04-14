using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {




        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
