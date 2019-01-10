using UnityEngine;
using UnityEngine.SceneManagement;

public class CajScenes : MonoBehaviour
{
    //========================================
    static public void showAction() {
        SceneManager.LoadScene("Action");
    }
    //========================================
    static public void showMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    //========================================
}
