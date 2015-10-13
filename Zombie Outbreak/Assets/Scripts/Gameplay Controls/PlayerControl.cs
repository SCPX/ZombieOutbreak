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
		CheckTouches();
		UpdatePosition();
	}

	void CheckTouches() {
		if(Input.touchCount <= 0) {
			PrintDebug("");
			return;
		}
		foreach(Touch touch in Input.touches) {
			bool consumed = false;
			if(RectTransformUtility.RectangleContainsScreenPoint(MovementPad, touch.position)) {
				Vector2 localPosition = Vector2.zero;
				if(RectTransformUtility.ScreenPointToLocalPointInRectangle(MovementPad, touch.position, null, out localPosition)) {
					Vector2 dir = localPosition - MovementPad.rect.center;
					float distance = Vector2.Distance(localPosition, MovementPad.rect.center);
					float radius = MovementPad.rect.width > MovementPad.rect.height ? MovementPad.rect.height / 2 : MovementPad.rect.width / 2;
					if(distance < radius/* && distance > radius / 10*/) {
						moveDirection = dir.normalized;
						consumed = true;
					}
				}
			}
			if(!consumed) {
				Vector3 touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));
				Vector3 diff = touchPoint - transform.position;
				diff.Normalize();

				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler (0f, 0f, rot_z - 90);
				FireWeapon(touchPoint);
			} 
		}
	}

	void UpdatePosition() {
		if(moveDirection != Vector3.zero) {
			Vector3 newLocation = transform.position + (moveDirection * speed);
			transform.position = Vector3.Lerp(transform.position, newLocation, speed * Time.deltaTime);
		}
	}
	
	private Color[] colors = new Color[12] {Color.red, Color.red, Color.blue, Color.blue, Color.yellow, Color.yellow, Color.green, Color.green, Color.white, Color.white, Color.cyan, Color.cyan};
	private int index = 0;
	public void FireWeapon(Vector3 point) {
		PrintDebug ("IMMA FIRIN MAH LAZER!!!!");
		Debug.DrawLine(transform.position, point, colors[index]);
		label.color = colors[index];
		index = (index + 1) % colors.Length;
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
