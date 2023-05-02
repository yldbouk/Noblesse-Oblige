using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Canvas blackOverlay;
    public bool debugMode;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main Manager Loaded");

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

        if (debugMode) GameObject.Find("debugmenu").GetComponent<Canvas>().enabled = true;
        else
        {
            Destroy(GameObject.Find("debugmenu"));
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }
    }

    public void DEBUGLoadLevel(string level)
    {
        GameObject.Find("debugmenu").gameObject.SetActive(false);
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
    }

    public void LoadNewLevel(string newLevel, string oldLevel)
    {
        Debug.Log("Unloading " + oldLevel + ", Loading " + newLevel);
        SceneManager.UnloadSceneAsync(oldLevel);
        SceneManager.LoadScene(newLevel, LoadSceneMode.Additive);

    }

    public float overlay
    {
        set
        {
            if (value > 1 || value < 0) throw new System.Exception("Must be between 0 and 1!");
            else blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, value);
        }
    }
    public void OverlayOn() { blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 1); }
    public void OverlayOff() { blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 0); }
    public  IEnumerator OverlayFadeIn (int ms = 500) { yield return OverlayFade(false,ms); }
    public  IEnumerator OverlayFadeOut(int ms = 500) { yield return OverlayFade(true, ms); }
    private IEnumerator OverlayFade(bool toBlack, int ms)
    {
        Debug.Log("Fading " + (toBlack ? "out" : "in") + " in " + ms + "ms");
        Image overlay = blackOverlay.GetComponent<Image>();
        for(int i = toBlack ? 0 : 255; toBlack ? (i<=255) : (i>=0);i += toBlack ? 1 : -1) {
            overlay.color = new Color(0, 0, 0, i/255f);
            yield return new WaitForSeconds((ms/255f)/1000);
        }
    }
}
