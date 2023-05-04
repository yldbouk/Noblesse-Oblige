using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollider : MonoBehaviour
{
    MainManager mainManager;
    private bool preventduplicates = false;

    private void Start() { mainManager = GameObject.Find("Manager").GetComponent<MainManager>(); }

   
    private void OnTriggerEnter2D(Collider2D collision) {
        if (preventduplicates) return; else preventduplicates = true;
        var scenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetSceneAt(1).buildIndex + 1);
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        mainManager.LoadNewLevel(scenePath.Substring(sceneNameStart, sceneNameLength));

    }
}
