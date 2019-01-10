using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    //========================================
    public void onClick() {
        CajApp.instance.restartAction();
    }
    //========================================
}
