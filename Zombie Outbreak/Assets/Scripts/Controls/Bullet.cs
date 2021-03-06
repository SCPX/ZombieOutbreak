﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public const float BULLET_LIFE_TIME = 5.0f;
	private Rigidbody2D _rigidbody;
	private float distanceTraveled = 0f;
	private Vector3 forward = Vector3.zero;

	public float speed = 30.0f; // in meters per second.
	public Gun source;

	void Start() {
		Destroy (this.gameObject, BULLET_LIFE_TIME);
		_rigidbody = GetComponent<Rigidbody2D>() as Rigidbody2D;
		if(_rigidbody == null) {
			_rigidbody = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
		}
		forward = new Vector3(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0.0f);
	}

	void FixedUpdate() {
		// Calculate distance that the bullet should move.
		Vector3 distance = (forward * speed * Time.fixedDeltaTime);
		// Move bullet
		_rigidbody.MovePosition(transform.position + distance);
		distanceTraveled += distance.magnitude;
		if(source != null && distanceTraveled > source.GetRange()) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == Constants.Strings.ENEMY_TAG) {
            // if we get funky results, need to convert V2 -> V3
            //Vector3 knockback = -(_rigidbody.velocity.normalized) * source.GetKnockback()*50;
			Vector3 knockback = forward * source.GetKnockback()*10;
            collision.gameObject.SendMessage("TakeDamage", source.GetDamage());
            collision.gameObject.SendMessage("ReceiveKnockback", knockback);

            // Let the enemy handle this?
        }
        Destroy(this.gameObject);
	}
}
