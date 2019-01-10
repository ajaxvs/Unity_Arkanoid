using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //========================================
    void Start() {
        Debug.Log("mainMenu start");
        if (SoundEnabler.getSoundState() != 0) {
            GetComponent<AudioSource>().Play();
        }
    }
    //========================================
    void Update() {
        if (Input.GetKeyDown(UnityEngine.KeyCode.Return)) {
            //default mode:
            CajScenes.showAction();
        } else if (Input.GetKeyDown(UnityEngine.KeyCode.Escape)) {
            //"Back" == "Escape":
#if (UNITY_WEBGL)
            //Debug.Log("webgl end, nn.");
#else
            //Debug.Log("mainMenu end, app quit.");
            Application.Quit();
#endif
        }
    }
    //========================================
}
