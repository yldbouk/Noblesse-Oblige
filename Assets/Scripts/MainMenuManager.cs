using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{ 
    public GameObject canvases;
    MainManager mainManager;

    public Button[] buttons = { };
/*  1 - Credits

*/

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main Menu Manager Loaded");

        // get reference to main mangger
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();

        // Set button listeners
        buttons[0].onClick.AddListener(delegate { switchViews("credits"); });
        buttons[1].onClick.AddListener(delegate { switchViews("main"); });

        // link cameras to canvases
        foreach (Canvas canvas in canvases.GetComponentsInChildren<Canvas>())
            canvas.worldCamera = Camera.main;

        // When all is done loading, fade in
        StartCoroutine(mainManager.OverlayFadeIn());

    }

    void switchViews(string name)
    {
        Vector2 position;
        switch (name)
        {
            case "credits": position = new Vector2(18,0); break;
            default:        position = new Vector2(0,0);  break;
        }
        Debug.Log("Switching menu view to " + name);
        Camera.main.transform.position = position;
    }

    public void PlayButtonPressed() { StartCoroutine("StartGame"); }

    IEnumerator StartGame()
    {
        Debug.Log("Play button pressed");

        // fade to black
        yield return StartCoroutine(mainManager.OverlayFadeOut(1000));

        // load next scene
        SceneManager.UnloadSceneAsync("MainMenu");


        

    }

}
