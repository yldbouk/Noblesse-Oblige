using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Canvas blackOverlay;
    private AudioSource bgm;

    public bool inCutscene = false;

    public bool hasDiedBefore = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main Manager Loaded");

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        bgm = Camera.main.GetComponent<AudioSource>();

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

    }

    public void DEBUGLoadMenu()
    {
        OverlayOn();
        SceneManager.UnloadSceneAsync(1);
        GameObject.Find("debugmenu").GetComponent<Canvas>().enabled = true;
    }

    public void DEBUGLoadLevel(string level)
    {
        GameObject.Find("debugmenu").gameObject.SetActive(false);
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        Cursor.visible = false;

    }

    public void LoadNewLevel(string newLevel)
    {
        Scene s = SceneManager.GetSceneAt(1);
        Debug.Log("Unloading " + s.name + ", Loading " + newLevel);
        SceneManager.UnloadSceneAsync(s);
        hasDiedBefore = false;
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
    public void OverlayOn(bool audioIgnore = false) 
    { 
        blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 1); 
        if (!audioIgnore) bgm.Pause();
    }
    public void OverlayOff(bool audioIgnore = false) 
    { 
        blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 0); 
        if (!audioIgnore) bgm.Play();
    }
    public  IEnumerator OverlayFadeIn (int ms = 500, bool audioIgnore = false) { yield return OverlayFade(false, ms, audioIgnore); }
    public  IEnumerator OverlayFadeOut(int ms = 500, bool audioIgnore = false) { yield return OverlayFade(true, ms, audioIgnore); }
    private IEnumerator OverlayFade(bool toBlack, int ms, bool audioIgnore)
    {
        Debug.Log("Fading " + (toBlack ? "out" : "in") + " in " + ms + "ms");
        Image overlay = blackOverlay.GetComponent<Image>();
        for(int i = toBlack ? 0 : 255; toBlack ? (i<=255) : (i>=0);i += toBlack ? 2 : -2) {
            overlay.color = new Color(0, 0, 0, i/255f);
            if (!audioIgnore) bgm.volume = (255 - i) / 255f * .3f;
            yield return new WaitForSecondsRealtime((ms/127.5f)/1000);
        }
    }

    public void PlayBGM(AudioClip c, float delay = .01f)
    {
        Debug.Log("Now Playing: " + c);
        bgm.Stop();
        bgm.clip = c;
        bgm.time = 0;
        bgm.PlayDelayed(delay);
    }

    public IEnumerator ResetLevel()
    {
        hasDiedBefore = true;
        yield return new WaitForSecondsRealtime(2);
        yield return OverlayFadeOut(1000);
        string s = SceneManager.GetSceneAt(1).name;
        Debug.Log("Reloading Scene "+s);
        SceneManager.UnloadSceneAsync(s).completed += 
            delegate { SceneManager.LoadScene(s, LoadSceneMode.Additive); };
        

    }

}
