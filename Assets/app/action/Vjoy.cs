using System;
using UnityEngine;

public class Vjoy : MonoBehaviour
{
    //========================================
    [SerializeField] private int xMoving = 0; //1000, -1000
    [SerializeField] private float alphaSpeed = 1.5f;
    //========================================
    private SpriteRenderer spRenderer;
    //========================================
    private void Start() {
#if (UNITY_EDITOR)
        Debug.Log("vjoy is alive - editor mode");
#else
        if (!Input.touchSupported) {
            Destroy(this.gameObject);
            return;
        }
#endif

        spRenderer = GetComponent<SpriteRenderer>();
    }
    //========================================
    void Update() {
        //fade:
        var color = spRenderer.color;
        color.a -= alphaSpeed * Time.deltaTime;
        spRenderer.color = color;
        //moving out of screen:
        var pos = transform.position;
        pos.x += xMoving * Time.deltaTime;
        transform.position = pos;
        if (Math.Abs(pos.x) > CajApp.instance.appWidth) {
            Destroy(this.gameObject);
        }
    }
    //========================================
}
