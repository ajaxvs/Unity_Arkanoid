using System;
using UnityEngine;

public class CameraAspectKeeper : MonoBehaviour
{
    //================================================================================
    [SerializeField] private Camera cameraObject = null;
    [SerializeField] private float gameAspectWidth = 16.0f;
    [SerializeField] private float gameAspectHeight = 9.0f;
    //================================================================================
    private int prevScreenWidth = -1;
    private int prevScreenHeight = -1;
    private Action onResize = null;
    //================================================================================
    void Start() {
        if (cameraObject == null) {
            var go = GameObject.Find("Main Camera");
            if (go != null) {
                cameraObject = go.GetComponent<Camera>();
            }
        }
    }
    //================================================================================
    public void setResizeListener(Action onResize) {
        this.onResize = onResize;
    }
    //================================================================================
    void Update() {
        if (cameraObject == null || gameAspectWidth <= 0.0f || gameAspectHeight <= 0.0f) return;

        if (Screen.width != prevScreenWidth || Screen.height != prevScreenHeight) {
            //save new size:
            prevScreenWidth = Screen.width;
            prevScreenHeight = Screen.height;
            Debug.Log("new screen size: " + prevScreenWidth + "x" + prevScreenHeight);

            //ShowAll:
            float gameAspect = gameAspectWidth / gameAspectHeight;
            float screenAspect = (float)Screen.width / Screen.height;
            float vScale = screenAspect / gameAspect;
            Rect rect = cameraObject.rect;
            if (vScale < 1.0f) {
                rect.width = 1.0f;
                rect.height = vScale;
                rect.x = 0;
                rect.y = (1.0f - vScale) / 2;
            } else {
                float hScale = 1.0f / vScale;
                rect.width = hScale;
                rect.height = 1.0f;
                rect.x = (1.0f - hScale) / 2;
                rect.y = 0;
            }
            cameraObject.rect = rect;

            //event:
            if (onResize != null) {
                onResize();
            }
        }
    }
    //================================================================================
}
