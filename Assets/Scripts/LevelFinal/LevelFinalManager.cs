using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinalManager : MonoBehaviour
{
    MainManager mainManager;
    LevelFinalPlayer player;
    LevelFinalBoss boss;

    [SerializeField] GameObject dialogueBG;
    [SerializeField] Text dialogueText;
    [SerializeField] Text nameText;
    [SerializeField] GameObject localOverlay;
    [Space]
    [SerializeField] AudioSource blipPlayer;
    [SerializeField] AudioSource blipBoss;
    [SerializeField] AudioSource audioTimeFrozen;
    [SerializeField] AudioSource audioHit;
    [Space]
    [SerializeField] GameObject[] DomainExpansionToDisable;
    [SerializeField] GameObject[] DomainExpansionToEnable;
    [Space]
    [SerializeField] AudioClip backgroundMusic;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Level Final Manager Loaded");
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelFinalPlayer>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<LevelFinalBoss>();

        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.GetComponent<AudioSource>().Stop();

        if (Input.GetKey(KeyCode.BackQuote)) mainManager.hasDiedBefore = true;

        mainManager.PlayBGM(backgroundMusic);
        if (mainManager.hasDiedBefore) StartCoroutine(CutsceneAlternative());
        else StartCoroutine(Cutscene());

    }

    IEnumerator WaitForDomainExpansion()
    {

        yield return new WaitForSecondsRealtime(100.6f);
        audioTimeFrozen.Play();
        mainManager.OverlayOn(true);

        dialogueBG.SetActive(false);
        foreach (var o in DomainExpansionToDisable) o.SetActive(false);
        foreach (var o in DomainExpansionToEnable) o.SetActive(true);

        yield return new WaitForSecondsRealtime(.65f);
        audioTimeFrozen.Play();
        Camera.main.AddComponent<CameraMovementSmooth>();
        mainManager.OverlayOff(true);
        mainManager.inCutscene = false;
        boss.enabled = true;
    }

    IEnumerator Cutscene()
    {        
        Camera.main.GetComponent<AudioSource>().volume = .3f;
        StartCoroutine(WaitForDomainExpansion());

        Debug.Log("Starting Cutscene");
        mainManager.inCutscene = true;
        yield return new WaitForSecondsRealtime(3);
        //Physics2D.IgnoreLayerCollision(6, 11, true);
        yield return mainManager.OverlayFadeIn(2000, true);
        yield return player.GoToWaypoint();
        yield return new WaitForSecondsRealtime(3);

        dialogueBG.SetActive(true);
        yield return PlayDialogue(Dialogue.bossFinal[0], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(1);

        yield return PlayDialogue(Dialogue.bossFinal[1], "Father?", blipBoss);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[2], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(1.5f);

        yield return PlayDialogue(Dialogue.bossFinal[3], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[4], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[5], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[6], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[7], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[8], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[9], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(1);

        yield return PlayDialogue(Dialogue.bossFinal[10], "Player", blipPlayer);
        yield return new WaitForSecondsRealtime(2);

        yield return PlayDialogue(Dialogue.bossFinal[11], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(1);
    }

    IEnumerator CutsceneAlternative()
    {
        mainManager.inCutscene = true;
        yield return new WaitForSecondsRealtime(1);
        //Physics2D.IgnoreLayerCollision(6, 11, true);
        yield return mainManager.OverlayFadeIn(500);
        yield return player.GoToWaypoint();
        yield return new WaitForSecondsRealtime(1);

        dialogueBG.SetActive(true);
        yield return PlayDialogue("Once Again!", "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(1);
        dialogueBG.SetActive(false);

        Camera.main.GetComponent<AudioSource>().time = 101.3f;

        foreach (var o in DomainExpansionToDisable) o.SetActive(false);
        foreach (var o in DomainExpansionToEnable) o.SetActive(true);
        Camera.main.AddComponent<CameraMovementSmooth>();
        mainManager.OverlayOff(true);
        mainManager.inCutscene = false;
        boss.enabled = true;
    }

    public IEnumerator BossKilled()
    {
        Debug.Log("Boss Killed");
        mainManager.inCutscene = true;
        player.GetComponent<Animator>().SetInteger("state", 0); // override just in case
        AudioSource bgm = Camera.main.GetComponent<AudioSource>();
        bgm.Pause();
        yield return new WaitForSecondsRealtime(4);

        dialogueBG.SetActive(true);
        yield return PlayDialogue(Dialogue.bossFinal[12], "Boss", blipBoss);
        yield return new WaitForSecondsRealtime(1);

        yield return mainManager.OverlayFadeOut(5000, true);
        localOverlay.SetActive(true);
        Text t = localOverlay.GetComponentInChildren<Text>();
        mainManager.OverlayOff(true);

        bgm.time = 213.3f;
        bgm.Play();

        yield return PlayDialogue(Dialogue.bossFinal[13], "Boss", blipBoss, .05f, true);
        yield return new WaitForSecondsRealtime(2.2f);

        t.text = Dialogue.credits[0];
        yield return new WaitForSecondsRealtime(5.4f);

        t.text = Dialogue.credits[1];
        yield return new WaitForSecondsRealtime(5.45f);

        t.text = Dialogue.credits[2];
        yield return new WaitForSecondsRealtime(6.1f);

        yield return mainManager.OverlayFadeOut(5000);

        Application.Quit();

    }

    IEnumerator PlayDialogue(string text, string name, AudioSource blip, float speed = .04f, bool shouldDisplayOnOverlay = false)
    {
        Text t = shouldDisplayOnOverlay ? GameObject.Find("Overlay").GetComponentInChildren<Text>() : dialogueText;

        nameText.text = name;
        t.text = "";

        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            t.text += chars[i];
            blip.Play();
            float s = speed;
            switch (chars[i])
            {
                //case ' ': s += .02f; break;
                case '"': continue;
                case ',': s *= 2; break;
                case '.':
                case '!':
                case '?': s *= 10; break;
            }

            yield return new WaitForSecondsRealtime(s);
        }
    }
}