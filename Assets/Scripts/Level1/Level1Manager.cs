using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour
{
    MainManager mainManager;
    Level1Player player;
    Level1Boss boss;

    [SerializeField] Collider2D[] collidersToEnableAfterCutscene;

    [SerializeField] GameObject dialogueBG;
    [SerializeField] Text dialogueText;
    [SerializeField] Text nameText;
    [SerializeField] GameObject tooltipText;
    [SerializeField] GameObject globalToolTipBG;
    [SerializeField] Text globalTooltipText;
    [Space]
    [SerializeField] AudioSource blipPlayer;
    [SerializeField] AudioSource blipBoss;
    [SerializeField] AudioSource audioTooltip;
    [SerializeField] AudioSource audioTimeFrozen;
    [SerializeField] AudioSource audioHit;
    [SerializeField] AudioSource audioAttack;
    [Space]
    [SerializeField] public AudioClip backgroundMusic;



    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Level1Player>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Level1Boss>();
        mainManager.PlayBGM(null);
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        
        mainManager.inCutscene = true;
        Physics2D.IgnoreLayerCollision(6, 11, true);
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

        StartCoroutine(boss.GoTo(player.transform.position));
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
        Camera.main.GetComponent<CameraGrayscale>().enabled = true;
        yield return new WaitForSeconds(4);

        audioTooltip.Play();
        tooltipText.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.LeftShift)) yield return null;

        audioAttack.Play();
        tooltipText.SetActive(false);
        //player.GetComponent<movement>().enabled = true;
        player.GetComponent<Animator>().speed = 1;
        boss.GetComponent<Animator>().speed = 1; 
        Camera.main.GetComponent<CameraGrayscale>().enabled = false;
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        {
            player.GetComponent<Animator>().SetTrigger("roll");
            playerRB.velocity = new Vector2(9, 0);
        }
        //boss.Attack();
        boss.readyToAttack = false;
        while(playerRB.velocity.x > 0) yield return null;
        yield return new WaitForSeconds(.5f);
        boss.animationState = 0;
        yield return new WaitForSeconds(2.5f);

        boss.GetComponent<SpriteRenderer>().flipX = true;
        Physics2D.IgnoreLayerCollision(6, 11, false);
        yield return new WaitForSeconds(1);

        dialogueBG.SetActive(true);
        yield return PlayDialogue(Dialogue.boss1[6], "Player", blipPlayer);
        yield return new WaitForSeconds(3);

        yield return PlayDialogue(Dialogue.boss1[7], "Boss", blipBoss);
        yield return new WaitForSeconds(3);
        
        dialogueBG.SetActive(false);
        yield return boss.GoTo(player.transform.position);
        yield return new WaitForSeconds(.5f);
        boss.animationState = 2;
        while (!boss.readyToAttack) yield return null;

        audioTimeFrozen.Play();
        player.GetComponent<Animator>().speed = 0;
        boss.GetComponent<Animator>().speed = 0;
        Camera.main.GetComponent<CameraGrayscale>().enabled = true;
        yield return new WaitForSeconds(1);

        audioTooltip.Play();
        tooltipText.transform.transform.position = new Vector2(1100, 182);
        tooltipText.GetComponent<Text>().text = "F to Attack";
        tooltipText.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.F)) yield return null;

        tooltipText.SetActive(false);
        player.GetComponent<Animator>().speed = 1;
        //boss.GetComponent<Animator>().speed = 1;
        Camera.main.GetComponent<CameraGrayscale>().enabled = false;

        player.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(.15f);

        audioHit.Play();
        globalToolTipBG.SetActive(true);
        boss.gameObject.SetActive(false);
        yield return new WaitForSeconds(4);

        audioTooltip.Play();
        globalTooltipText.text = "You Killed Your Brother";
        yield return new WaitForSeconds(5);
        audioTooltip.Play();
        globalTooltipText.text = "Congratulations";
        yield return new WaitForSeconds(4);

        globalToolTipBG.SetActive(false); 
        foreach (Collider2D c in collidersToEnableAfterCutscene) c.enabled = true;
        mainManager.inCutscene = false;

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
                case ' ': s += .02f; break;
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
