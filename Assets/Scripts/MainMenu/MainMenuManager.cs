using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{ 
    public Canvas canvas;
    MainManager mainManager;
    private List<string> views;

    [SerializeField] AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main Menu Manager Loaded");

        // get reference to main mangger
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();

        // play bgm
        mainManager.PlayBGM(backgroundMusic, .5f);

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
    public void ButtonPressed(string op)
    {
        switch (op)
        {
            case "play": StartCoroutine(StartGame()); break;
            case "exit": StartCoroutine(ExitGame()); break;
        }
    }
    
    IEnumerator StartGame()
    {
        Debug.Log("Play button pressed");

        // fade to black
        yield return StartCoroutine(mainManager.OverlayFadeOut(1000));

        // load next scene
        mainManager.LoadNewLevel("Prologue");
    }
    IEnumerator ExitGame()
    {
        Debug.Log("Exiting Game...");
        yield return mainManager.OverlayFadeOut(3000);
        Application.Quit(0);
    }

}

