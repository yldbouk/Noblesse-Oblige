using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCutsceneManager : MonoBehaviour
{
    private string[] prologueParagraphs =
    {
        "In the land of [KINGDOM NAME], nestled deep within the rolling hills and verdant forests, a great kingdom flourished. Its people were renowned for their skill in archery and swordsmanship, and their shining cities boasted towering spires and grand castles. Prince [PRINCE NAME] was the kingdom's greatest champion, his noble lineage and formidable combat skills earning him the respect of all who knew him.",
        "But when war came knocking at [KINGDOM NAME]'s door, the kingdom could not turn a blind eye to the plight of its ally. The call to arms was sounded, and [PRINCE NAME] answered without hesitation, leading the kingdom's finest warriors to the front lines.",
        "The war was a long and grueling struggle, but [PRINCE NAME]'s leadership and bravery proved crucial to his side's eventual victory. His sword and shield were stained with the blood of the enemy, and his armor was dented and scarred from countless battles. But he emerged from the conflict a hero, and the kingdom celebrated his triumph upon his return.",
        "As [PRINCE NAME] rode his white stallion through the countryside, the weight of the war still hung heavy on his mind. He had seen much death and destruction, and the scars left by the conflict would never fully heal. But he was proud of what he had accomplished, and knew that his kingdom was stronger for having fought alongside its allies.",
        "As [PRINCE NAME] rode closer to his kingdom, he began to notice something odd. The usually bustling towns and villages were eerily quiet, and there was no one around to greet him upon his triumphant return.",
        "As he rode past the kingdom's walls, he was met with a scene of utter devastation. The once-great city he called home lay in ruins, and small fires dotted the landscape.",
        "Bodies were strewn about, and the smell of death hung heavy in the air. The sight was almost too much for [PRINCE NAME] to bear, and his heart sank as he realized that his brothers, staying behind to protect the kingdom, failed.",
        "Without hesitation, he spurred his horse forward, his mind racing with questions and his heart heavy with grief. He needed to find someone, anyone, who could explain what had happened.",
        "As he made his way through the ruined streets, he searched for any signs of life. But the kingdom was silent, and there was no one to be found. Eventually, he arrived at the great castle that had once been the heart of his kingdom.",
        "As he rushed through the castle gates, he was met with a sight that he would never forget. The great halls that had once been filled with life and laughter were now empty and silent.",
        "The walls were scorched and blackened, and the few surviving guards, unmoving, looked up at him with haunted eyes.",
        "Desperate for answers, [PRINCE NAME] hurried to convene with the kingdom's top officials. But when he arrived at the council chambers, he found them empty and abandoned."
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

        // remove prologue overlay
        Destroy(prologueText.transform.parent.gameObject);

        // enable the world
        canvas.transform.Find("BlackBars").gameObject.SetActive(true);

        // update references
        prologueText = canvas.transform.Find("BlackBars").GetComponentInChildren<Text>();
        blip = prologueText.gameObject.GetComponent<AudioSource>();
        GameObject.FindGameObjectWithTag("parallax").AddComponent<OpeningCutsceneParallax>();

        // scene 2... action
        yield return mainManager.OverlayFadeIn(1000);

        // we are now viewing the world
        // TODO: animations

        yield return DisplayCharacterByCharacter(prologueParagraphs[4], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[5], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[6], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[7], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[8], true);
        yield return new WaitForSeconds(5);

        yield return DisplayCharacterByCharacter(prologueParagraphs[9], true);
        yield return new WaitForSeconds(5);
        

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
