using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollider : MonoBehaviour
{
    MainManager mainManager;
    private bool preventduplicates = false;
    [SerializeField] int fadeOut = 0;

    private void Start() { mainManager = GameObject.Find("Manager").GetComponent<MainManager>(); }

   
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        if (preventduplicates) return; else preventduplicates = true;
        collision.gameObject.SetActive(false);
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        if (fadeOut == 0) mainManager.OverlayOn(true);
        else yield return mainManager.OverlayFadeOut(fadeOut);
        var scenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetSceneAt(1).buildIndex + 1);
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        mainManager.LoadNewLevel(scenePath.Substring(sceneNameStart, sceneNameLength));
    }
}
