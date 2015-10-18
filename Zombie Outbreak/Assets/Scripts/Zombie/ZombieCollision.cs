using UnityEngine;
using System.Collections;

public class ZombieCollision : MonoBehaviour {
    
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
            Debug.Log("killed the player");
        }else if(col.gameObject.tag == "PlayerBullet")
        {
            ZombieStats zombie;
            Bullet b = col.gameObject.GetComponent<Bullet>();

            float damage = b.source.GetDamage();
            float knockback = b.source.GetKnockback();
            zombie = GetComponent<ZombieStats>();
            zombie.health = zombie.health - damage;
            if (zombie.health <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
