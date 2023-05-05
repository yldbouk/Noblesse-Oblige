using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{

    MainManager mainManager;
    public Canvas canvas;
    Text prologueText;
    private AudioSource blip;
    [SerializeField] AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {    
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        prologueText = canvas.transform.Find("Prologue").GetComponentInChildren<Text>();
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        blip = prologueText.gameObject.GetComponent<AudioSource>();
        mainManager.PlayBGM(backgroundMusic, 2);

        canvas.transform.Find("BlackBars").gameObject.SetActive(false);

        StartCoroutine(startCutScene());
    }

    IEnumerator waitForInput() { while (Input.GetMouseButtonDown(0)) yield return null; while (!Input.GetMouseButtonDown(0)) yield return null; }

    IEnumerator startCutScene()
    {
        yield return mainManager.OverlayFadeIn();
        yield return new WaitForSeconds(.5f);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[0]);
        yield return waitForInput();

        yield return DisplayCharacterByCharacter(Dialogue.prologue[1]);
        yield return waitForInput();

        prologueText.text = "";
        yield return new WaitForSeconds(.25f);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[2]);
        yield return waitForInput();

        yield return DisplayCharacterByCharacter(Dialogue.prologue[3]);
        yield return waitForInput();

        yield return mainManager.OverlayFadeOut(1000, true);

        // remove prologue overlay
        Destroy(prologueText.transform.parent.gameObject);

        // enable the world
        canvas.transform.Find("BlackBars").gameObject.SetActive(true);
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Player")) c.GetComponent<SpriteRenderer>().enabled = true;

        // update references
        prologueText = canvas.transform.Find("BlackBars").GetComponentInChildren<Text>();
        blip = prologueText.gameObject.GetComponent<AudioSource>();
        GameObject.FindGameObjectWithTag("parallax").AddComponent<PrologueParallax>();

        // scene 2... action
        yield return mainManager.OverlayFadeIn(1000, true);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[4], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[5], true);
        yield return new WaitForSeconds(4);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[6], true);
        yield return new WaitForSeconds(4);

        yield return DisplayCharacterByCharacter(Dialogue.prologue[7], true);
        yield return new WaitForSeconds(4);

        yield return mainManager.OverlayFadeOut(3000);
        mainManager.LoadNewLevel("Level1");

    }

    IEnumerator DisplayCharacterByCharacter(string text, bool shouldClear = false, float speed = .04f)
    {
        char[] chars = text.ToCharArray();

        if (shouldClear) prologueText.text = "";
        for (int i=0; i<chars.Length; i++)
        {
            prologueText.text += chars[i];

            if(Input.GetMouseButton(0) && i > 4) continue;

            blip.Play();

            float s = speed;
            
            switch (chars[i])
            {
                case ' ': s += .05f; break;
                case ',': s += .1f;  break;
                case '.': s += .2f;  break;
            }
 
            yield return new WaitForSeconds(s);
        }
        prologueText.text += "\n\n";
    }

}
