using UnityEngine;
using UnityEngine.UI;

public class SoundEnabler : MonoBehaviour
{
    //========================================
    static public string prefsKey = "soundEnable";
    //========================================
    [SerializeField] private Button butSound = null;
    [SerializeField] private Sprite imgOn = null;
    [SerializeField] private Sprite imgOff = null;
    //========================================
    static private bool currentState = true;
    //========================================
    void Start() {
        setSound(getSoundState());
    }
    //========================================
    static public void saveSoundState(int mode) {
        currentState = (mode != 0) ? true : false;
        PlayerPrefs.SetInt(prefsKey, mode);
    }
    //========================================
    static public int getSoundState() {
        return PlayerPrefs.GetInt(prefsKey, 1);
    }
    //========================================
    static public bool isSoundEnabled() {
        return currentState;
    }
    //========================================
    /// <summary>
    /// Changes button image, saves state into the PlayerPrefs.
    /// </summary>
    /// <param name="state">off = 0; on = 1; switch = -1</param>
    public void setSound(int state = -1) {
        if (state == -1) {
            state = (getSoundState() == 0) ? 1 : 0;
        }
        if (butSound != null) {
            if (state == 0) {
                butSound.image.overrideSprite = imgOff;
            } else {
                butSound.image.overrideSprite = imgOn;
            }
        }
        saveSoundState(state);
    }
    //========================================
    public void switchSound() {
        setSound(-1);
    }
    //========================================
}
