using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] AudioSource blipPlayer;
    [SerializeField] AudioSource blipBoss;
    [SerializeField] AudioSource audioTimeFrozen;
    [SerializeField] AudioSource audioHit;

    [SerializeField] GameObject[] DomainExpansionToDisable;
    [SerializeField] GameObject[] DomainExpansionToEnable;


    public void DEBUGSkipCutscene()
    {        
        dialogueBG.SetActive(false);
        foreach (var o in DomainExpansionToDisable) o.SetActive(false);
        foreach (var o in DomainExpansionToEnable) o.SetActive(true);
        mainManager.OverlayOff();
        mainManager.inCutscene = false;
        boss.enabled = true;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Level Final Manager Loaded");

        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelFinalPlayer>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<LevelFinalBoss>();

        if (mainManager.debugMode && Input.GetKey(KeyCode.RightShift)) { DEBUGSkipCutscene(); return; }

  
        StartCoroutine(WaitForDomainExpansion());
        StartCoroutine(Cutscene());

        
    }

    IEnumerator WaitForDomainExpansion()
    {

        yield return new WaitForSecondsRealtime(100.7f);
        audioTimeFrozen.Play();
        mainManager.OverlayOn();

        dialogueBG.SetActive(false);
        foreach (var o in DomainExpansionToDisable) o.SetActive(false);
        foreach (var o in DomainExpansionToEnable) o.SetActive(true);

        yield return new WaitForSecondsRealtime(.55f);
        audioTimeFrozen.Play();
        mainManager.OverlayOff();
        mainManager.inCutscene = false;
        boss.enabled = true;
    }


    IEnumerator Cutscene()
    {
        Debug.Log("Starting Cutscene");
        mainManager.inCutscene = true;
        yield return new WaitForSeconds(3); 
        //Physics2D.IgnoreLayerCollision(6, 11, true);
        yield return mainManager.OverlayFadeIn(2000);
        yield return player.GoToWaypoint();
        yield return new WaitForSeconds(3);

        dialogueBG.SetActive(true);
        yield return PlayDialogue(Dialogue.bossFinal[0], "Player", blipPlayer);
        yield return new WaitForSeconds(1);

        yield return PlayDialogue(Dialogue.bossFinal[1], "Father?", blipBoss);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[2], "Player", blipPlayer);
        yield return new WaitForSeconds(1.5f);

        yield return PlayDialogue(Dialogue.bossFinal[3], "Boss", blipBoss);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[4], "Player", blipPlayer);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[5], "Boss", blipBoss);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[6], "Boss", blipBoss);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[7], "Player", blipPlayer);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[8], "Player", blipPlayer);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[9], "Boss", blipBoss);
        yield return new WaitForSeconds(1);

        yield return PlayDialogue(Dialogue.bossFinal[10], "Player", blipPlayer);
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.bossFinal[11], "Boss", blipBoss);
        yield return new WaitForSeconds(1);
    }


    IEnumerator PlayDialogue(string text, string name, AudioSource blip, float speed = .04f)
    {
        nameText.text = name;
        dialogueText.text = "";

        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            dialogueText.text += chars[i];
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

            yield return new WaitForSeconds(s);
        }
    }
}