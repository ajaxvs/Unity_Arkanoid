using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtons : MonoBehaviour
{
    //========================================
    public void startEasy() {
        startAction(CajApp.ActionMode.ACTION_MODE_EASY);
    }
    //========================================
    public void startClassic() {
        startAction(CajApp.ActionMode.ACTION_MODE_CLASSIC);
    }
    //========================================
    private void startAction(CajApp.ActionMode mode) {
        CajApp.actionMode = mode;
        CajScenes.showAction();
    }
    //========================================
}
