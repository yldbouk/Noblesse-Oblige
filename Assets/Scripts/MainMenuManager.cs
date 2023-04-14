using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button[] buttons = { };
/*  1 - Credits

*/
    // Start is called before the first frame update
    void Start()
    {
        buttons[0].onClick.AddListener(delegate { switchViews("credits"); });
        buttons[1].onClick.AddListener(delegate { switchViews("main"); });


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void switchViews(string name)
    {
        Vector2 position;
        switch (name)
        {
            case "credits": position = new Vector2(18,0); break;
            default:        position = new Vector2(0,0);  break;
        }
        Debug.Log("Switching menu view to " + name);
        Camera.main.transform.position = position;
    }
     
}
