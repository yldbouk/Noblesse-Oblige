using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour
{
    MainManager mainManager;
    Level1Player player;
    Level1Boss boss;


    [SerializeField] GameObject dialogueBG;
    [SerializeField] Text dialogueText;
    [SerializeField] Text nameText;
    [SerializeField] GameObject tooltipText;

    [SerializeField] AudioSource blipPlayer;
    [SerializeField] AudioSource blipBoss;
    [SerializeField] AudioSource audioTooltip;
    [SerializeField] AudioSource audioTimeFrozen;


    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Level1Player>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Level1Boss>();

        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        yield return mainManager.OverlayFadeIn(2000);
        yield return player.GoToWaypoint();
        yield return new WaitForSeconds(1);

        dialogueBG.SetActive(true);
        yield return PlayDialogue(Dialogue.boss1[0], "Player", blipPlayer);
        
        yield return new WaitForSeconds(2);

        yield return PlayDialogue(Dialogue.boss1[1], "Brother", blipBoss);
        yield return new WaitForSeconds(3);

        yield return PlayDialogue(Dialogue.boss1[2], "Player", blipPlayer);
        yield return new WaitForSeconds(2);

        StartCoroutine(boss.GoToWaypoint());
        yield return PlayDialogue(Dialogue.boss1[3], "Brother", blipBoss);
        yield return new WaitForSeconds(3);

        yield return PlayDialogue(Dialogue.boss1[4], "Player", blipPlayer, .08f);
        yield return new WaitForSeconds(3);

        yield return PlayDialogue(Dialogue.boss1[5], "Brother", blipBoss, .8f);
        yield return new WaitForSeconds(.5f);

        dialogueBG.SetActive(false);
        boss.animationState = 2;
        while (!boss.readyToAttack) yield return null;

        audioTimeFrozen.Play();
        player.GetComponent<Animator>().speed = 0;
        boss.GetComponent<Animator>().speed = 0;
        Camera.main.GetComponent<CameraScript>().grayscale = true;
        yield return new WaitForSeconds(4);

        audioTooltip.Play();
        tooltipText.SetActive(true);

        while (!Input.GetMouseButtonDown(0)) yield return null;

        audioTimeFrozen.Play();
        tooltipText.SetActive(false);
        player.GetComponent<Animator>().speed = 1;
        boss.GetComponent<Animator>().speed = 1; 
        Camera.main.GetComponent<CameraScript>().grayscale = false;
        boss.Attack();
        yield return new WaitForSeconds(.2f);
        boss.animationState = 0;



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
                //case ' ': s += .05f; break;
                case ',': s *= 2; break;
                case '.':
                case '!':
                case '?': s *= 10; break;          
            }

            yield return new WaitForSeconds(s);
        }
    }
}
