using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCutSceneManager : MonoBehaviour
{
    private string[] prologueParagraphs =
    {
        "In the land of [KINGDOM NAME], nestled deep within the rolling hills and verdant forests, a great kingdom flourished. Its people were renowned for their skill in archery and swordsmanship, and their shining cities boasted towering spires and grand castles. Prince [PRINCE NAME] was the kingdom's greatest champion, his noble lineage and formidable combat skills earning him the respect of all who knew him.",
        "But when war came knocking at [KINGDOM NAME]'s door, the kingdom could not turn a blind eye to the plight of its ally. The call to arms was sounded, and [PRINCE NAME] answered without hesitation, leading the kingdom's finest warriors to the front lines.",
        "The war was a long and grueling struggle, but [PRINCE NAME]'s leadership and bravery proved crucial to his side's eventual victory. His sword and shield were stained with the blood of the enemy, and his armor was dented and scarred from countless battles. But he emerged from the conflict a hero, and the kingdom celebrated his triumph upon his return.",
        "As [PRINCE NAME] rode his white stallion through the countryside, the weight of the war still hung heavy on his mind. He had seen much death and destruction, and the scars left by the conflict would never fully heal. But he was proud of what he had accomplished, and knew that his kingdom was stronger for having fought alongside its allies."
    };

    MainManager mainManager;

    public Canvas canvas;

    Text prologueText;

    // Start is called before the first frame update
    void Start()
    {    
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        prologueText = canvas.transform.Find("Prologue").GetComponentInChildren<Text>();
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();

        StartCoroutine(startCutScene());
    }

    IEnumerator waitForInput() { while (Input.GetMouseButtonDown(1)) yield return null; while (!Input.GetMouseButtonDown(1)) yield return null; }

    IEnumerator startCutScene()
    {
        yield return mainManager.OverlayFadeIn();
        yield return new WaitForSeconds(.5f);
        yield return Prologue();

    }

    IEnumerator Prologue()
    {
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

        Destroy(prologueText.transform.parent.gameObject);
        canvas.transform.Find("BlackBars").gameObject.SetActive(true);
        mainManager.OverlayOff();
    }

    IEnumerator DisplayCharacterByCharacter(string text, float speed = .04f)
    {
        foreach (char l in text.ToCharArray())
        {
            prologueText.text += l;
            prologueText.gameObject.GetComponent<AudioSource>().Play();

            float s = speed;
            
            switch (l)
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
