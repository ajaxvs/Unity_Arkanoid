using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    //================================================================================
    public Text textField = null;
    public float updateTime = 1.0f; //seconds.
    //================================================================================
    private float timePassed = 0;
    private int frames = 0;
    private int lastFPS = 0;
    //================================================================================
    void Start() {

    }
    //================================================================================
    void Update() {
        timePassed += Time.deltaTime;
        frames++;
        if (timePassed > updateTime) {
            lastFPS = (int)(frames / timePassed);
            if (textField != null) {
                textField.text = lastFPS.ToString();
            }
            timePassed = 0.0f;
            frames = 0;
        }
    }
    //================================================================================
}
