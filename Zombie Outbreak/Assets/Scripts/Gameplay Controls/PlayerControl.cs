using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public GameObject bullet;
	public UnityEngine.UI.Text label;
	public RectTransform MovementPad;
	public float speed = 0.1f;

	private int movementFingerIndex = -1;
	private Vector3 moveDirection = Vector3.zero;

	void Start() {
		if(label == null) {
			Debug.LogError("Missing bullet prefab!");
			Destroy (this);
			return;
		}
	}

	void Update() {
		moveDirection = Vector3.zero;
		CheckMovementPad();
		UpdatePosition();
	}

	void CheckTouches() {
		if(Input.touchCount > 0) {
			foreach(Touch touch in Input.touches) {
				// Check where touch is, and whether it is in a "no-fire" zone or not.
				// Need to check if it's on a button or not.
				// Need to check if it's on the movement pad or not.
				// If available to fire, take the shot. 
			}
		}
	}

	void CheckMovementPad() {
		foreach(Touch touch in Input.touches) {
			if(RectTransformUtility.RectangleContainsScreenPoint(MovementPad, touch.position)) {
				Vector2 localPosition = Vector2.zero;
				if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(MovementPad, touch.position, null, out localPosition))
					continue;
				
				Vector2 dir = localPosition - MovementPad.rect.center;
				float distance = Vector2.Distance(localPosition, MovementPad.rect.center);
				float minSide = MovementPad.rect.width > MovementPad.rect.height ? MovementPad.rect.height : MovementPad.rect.width;
				if(distance < minSide / 2) {
					float adjustedDistance = distance / (minSide / 2);
					moveDirection = dir.normalized * adjustedDistance;
					PrintDebug("Rect: " + MovementPad.rect.ToString() + ", localPos: " + localPosition + ", Center: " + MovementPad.rect.center + ", Dir: " + dir + ", distance: " + distance + ", minSide: " + minSide + ", adjustedDistance: " + adjustedDistance + ", moveDirection: " + moveDirection);
				}
			}
		}
	}
	
	void UpdatePosition() {
		if(moveDirection != Vector3.zero) {
			Vector3 newLocation = transform.position + (moveDirection * speed);
			transform.position = Vector3.Lerp(transform.position, newLocation, speed * Time.deltaTime);
		}
	}

	void PrintDebug(string msg) {
#if UNITY_EDITOR
		Debug.Log (msg);
		label.text = msg;
#endif
	}

#if UNITY_EDITOR
	void CheckDebugMovement() {

	}

	void CheckDebugFire() {

	}
#endif
}
