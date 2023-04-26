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

        if (debugMode)  GameObject.Find("debugmenu").GetComponent<Canvas>().enabled = true;
        else
        {
            Destroy(GameObject.Find("debugmenu"));
            LoadLevel("MainMenu");
        }
    }

    public void LoadLevel(string level)
    {
        GameObject.Find("debugmenu").gameObject.SetActive(false);
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
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
