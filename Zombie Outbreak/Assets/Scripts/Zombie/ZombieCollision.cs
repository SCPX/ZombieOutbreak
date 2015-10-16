using UnityEngine;
using System.Collections;

public class ZombieCollision : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("killed the player");
        }else if(col.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
        }

    }
}
