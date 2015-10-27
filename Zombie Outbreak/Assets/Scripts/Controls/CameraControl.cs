using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public float distance = 40.0f;

	public bool SmoothTransition = false;
	public float speed = 30.0f;

	private Transform player;

	void Start () {
		player = PlayerControl.Instance.transform;
		if(player == null) 
			Destroy (this);
		else
			transform.position = new Vector3(player.position.x, player.position.y, -distance);
	}
	
	void Update () {
		if(player != null) {
			// Reposition to be above the player.
			Vector3 targetPosition = new Vector3(player.position.x, player.position.y, -distance);
			if(SmoothTransition) {
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
			} else {
				transform.position = targetPosition;
			}
		}
	}
}
