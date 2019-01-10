using System;
using System.Diagnostics;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    //========================================
    [SerializeField] private GameObject colliderForAnimationStart = null;
    [SerializeField] private GameObject animatorHolder = null;
    private string animationOnHit = "p1OnHitAnim";
    //========================================
    private readonly float moveSpeed = 50.0f * 0.02f * 1000;
    
    private Rigidbody2D rig;
    private Animator animatorOnHit = null;
    //========================================
    void Start() {
        rig = GetComponent<Rigidbody2D>();
        animatorOnHit = animatorHolder.GetComponent<Animator>();
    }
    //========================================
    void FixedUpdate() {
        float dx = 0.0f;
        updateMovementAxis(ref dx);
        updateMovementTouches(ref dx);
        updateMovementClicks(ref dx);
        moveRig(dx);
    }
    //========================================
    //[Conditional("UNITY_STANDALONE"), Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
    private void updateMovementAxis(ref float dx) {
        dx = Input.GetAxisRaw("Horizontal");
    }
    //========================================
    //[Conditional("UNITY_ANDROID"), Conditional("UNITY_WEBGL")]
    private void updateMovementTouches(ref float dx) {
        if (Math.Abs(dx) < float.Epsilon && Input.touches.Length > 0) {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary
                ) {
                dx = 1.0f;
                if (touch.position.x < Screen.width / 2) {
                    dx = -1.0f;
                }
            }
        }
    }
    //========================================
    [Conditional("UNITY_EDITOR")]
    private void updateMovementClicks(ref float dx) {
        if (Math.Abs(dx) < float.Epsilon && Input.GetMouseButton(0)) {
            dx = 1.0f;
            if (Input.mousePosition.x < Screen.width / 2) {
                dx = -1.0f;
            }
        }
    }
    //========================================
    private void moveRig(float dx) {
        var v = rig.velocity;
        v.x = dx * moveSpeed;
        rig.velocity = v;
    }
    //========================================
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == colliderForAnimationStart) {
            if (animatorOnHit != null) {
                animatorOnHit.Play(animationOnHit, -1, 0.0f);
            }
        }
    }
    //========================================
}
