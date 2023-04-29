using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{
    private readonly string[] prologueParagraphs =
    {
        "In the land of [KINGDOM NAME], nestled deep within the rolling hills and verdant forests, a great kingdom flourished. Its people were renowned for their skill in archery and swordsmanship, and their shining cities boasted towering spires and grand castles. Prince [PRINCE NAME] was the kingdom's greatest champion, his noble lineage and formidable combat skills earning him the respect of all who knew him.",
        "But when war came knocking at [KINGDOM NAME]'s door, the kingdom could not turn a blind eye to the plight of its ally. The call to arms was sounded, and [PRINCE NAME] answered without hesitation, leading the kingdom's finest warriors to the front lines.",
        "The war was a long and grueling struggle, but [PRINCE NAME]'s leadership and bravery proved crucial to his side's eventual victory. His sword and shield were stained with the blood of the enemy, and his armor was dented and scarred from countless battles. But he emerged from the conflict a hero, and the kingdom celebrated his triumph upon his return.",
        "As [PRINCE NAME] rode his white stallion through the countryside, the weight of the war still hung heavy on his mind. He had seen much death and destruction, and the scars left by the conflict would never fully heal. But he was proud of what he had accomplished, and knew that his kingdom was stronger for having fought alongside its allies.",
        "As [PRINCE NAME] rode closer to his kingdom, he began to notice something odd. The usually bustling towns and villages were eerily quiet, and there was no one around to greet him upon his triumphant return.",
        "When he rode past the kingdom's walls, he was met with a scene of utter devastation. The once-great city he called home lay in ruins, with small fires dotting the landscape.",
        "Bodies were strewn about, and the smell of death hung heavy in the air. The sight was almost too much for [PRINCE NAME] to bear, and his heart sank as he realized that his brothers, staying behind to protect the kingdom, had failed.",
        "He needed to find someone, anyone, who could explain what had happened. But the castle was empty and abandoned. All that remained was one servant in the middle of the main hall."
    };

    MainManager mainManager;

    public Canvas canvas;

    Text prologueText;

    private AudioSource blip;

    // Start is called before the first frame update
    void Start()
    {    
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        prologueText = canvas.transform.Find("Prologue").GetComponentInChildren<Text>();
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        blip = prologueText.gameObject.GetComponent<AudioSource>();

        canvas.transform.Find("BlackBars").gameObject.SetActive(false);

        StartCoroutine(startCutScene());
    }

    IEnumerator waitForInput() { while (Input.GetMouseButtonDown(0)) yield return null; while (!Input.GetMouseButtonDown(0)) yield return null; }

    IEnumerator startCutScene()
    {
        yield return mainManager.OverlayFadeIn();
        yield return new WaitForSeconds(.5f);

        yield return DisplayCharacterByCharacter(prologueParagraphs[0]);
        yield return waitForInput();

        yield return DisplayCharacterByCharacter(prologueParagraphs[1]);
        yield return waitForInput();

        prologueText.text = "";
        yield return new WaitForSeconds(.25f);

        yield return DisplayCharacterByCharacter(prologueParagraphs[2]);
        yield return waitForInput();

        yield return DisplayCharacterByCharacter(prologueParagraphs[3]);
        yield return waitForInput();

        yield return mainManager.OverlayFadeOut(1000);

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
        yield return mainManager.OverlayFadeIn(1000);

        yield return DisplayCharacterByCharacter(prologueParagraphs[4], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[5], true);
        yield return new WaitForSeconds(4);

        yield return DisplayCharacterByCharacter(prologueParagraphs[6], true);
        yield return new WaitForSeconds(4);

        yield return DisplayCharacterByCharacter(prologueParagraphs[7], true);
        yield return new WaitForSeconds(4);

        yield return mainManager.OverlayFadeOut(3000);

    }

    IEnumerator DisplayCharacterByCharacter(string text, bool shouldClear = false, float speed = .04f)
    {
        char[] chars = text.ToCharArray();

        if (shouldClear) prologueText.text = "";
        for (int i=0; i<chars.Length; i++)
        {
            prologueText.text += chars[i];

            if(Input.GetMouseButton(0) && i > 4)
            {
                yield return new WaitForSeconds(.005f);
                continue;
            }

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
