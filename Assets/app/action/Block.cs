using UnityEngine;

public class Block : MonoBehaviour
{
    //========================================
    
    //========================================
    void Start() {
        
    }
    //========================================
    private void OnCollisionEnter2D(Collision2D collision) {
        CajApp.instance.onBlockDestroy(collision);
        Destroy(this.gameObject);     
    }
    //========================================
}
