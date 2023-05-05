using UnityEngine;

public class LevelGenericManager : MonoBehaviour
{
    MainManager mainManager;

    void Start()
    {
        mainManager = GameObject.Find("Manager").GetComponent<MainManager>();
        mainManager.OverlayOff(true);
    }
}