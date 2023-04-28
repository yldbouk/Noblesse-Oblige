using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{ 
    public Canvas canvas;
    MainManager mainManager;
    private List<string> views;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main Menu Manager Loaded");

        // get reference to main mangger
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();

        // link camera to canvas
        canvas.worldCamera = Camera.main;

        // switchViews safety 
        views = new List<string>();
        foreach (Transform t in canvas.transform) views.Add(t.name);
        switchViews(views[0]);

        // When all is done loading, fade in
        StartCoroutine(mainManager.OverlayFadeIn());

    }

    public void switchViews(string view)
    {
        if (!views.Contains(view)) { 
            Debug.Log("\"" + view + "\" is an invalid view. Defaulting to \"" + views[0] + "\"");
            view = views[0];
        } else Debug.Log("Switching menu view to " + view);
        foreach (Transform t in canvas.transform) t.gameObject.SetActive(t.name == view);
    }

    public void PlayButtonPressed() {  }

    IEnumerator StartGame()
    {
        Debug.Log("Play button pressed");

        // fade to black
        yield return StartCoroutine(mainManager.OverlayFadeOut(1000));

        // load next scene
        SceneManager.UnloadSceneAsync("MainMenu");


        

    }

}
