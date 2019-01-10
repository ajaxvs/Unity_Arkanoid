using UnityEngine;

public class Ball : MonoBehaviour
{
    //========================================
    private readonly float speed = 50.0f * 0.02f * 1000;
    //========================================
    private Rigidbody2D rig;
    //========================================
    void Start() {
        rig = GetComponent<Rigidbody2D>();
        restart();
    }
    //========================================
    public void restart() {
        rig.velocity = Vector2.down * speed;
    }
    //========================================
    void FixedUpdate() {
        if (rig.position.y < -CajApp.instance.appHeight / 2) {
            gameOver();
        }
    }
    //========================================
    private void gameOver() {
        this.gameObject.SetActive(false);
        CajApp.instance.onGameOver();
    }
    //========================================
    private void OnCollisionEnter2D(Collision2D collision) {
        //p1:
        if (collision.gameObject == CajApp.instance.player1) {
            //change move vector:
            float dx = (this.transform.position.x - collision.gameObject.transform.position.x) / 
                       collision.collider.bounds.size.x;
            var mv = new Vector2(dx, 1).normalized;
            rig.velocity = mv * speed;
            //sound:
            CajApp.instance.playHitSound(0.25f);
        }
    }
    //========================================
}
