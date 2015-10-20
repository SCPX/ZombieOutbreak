using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    public static PlayerControl Instance;
	public Text label;
	public RectTransform MovementPad;
	public float speed = 0.1f;

	private int movementFingerIndex = -1;
	private Vector3 moveDirection = Vector3.zero;
	private Weapon equippedWeapon;
	private float health = 1.0f;
	
	public float Health {
		get{ return health; }
		set{
			health = Mathf.Clamp(value, 0f, 1.0f);
			if(health <= 0f) {
				// Kill player
				KillPlayer();
			}
		}
	}

	private Rigidbody2D _rigidbody;
    void Awake()
    {
        Instance = this;
    }

	void Start() {
		if(label == null || MovementPad == null) {
			Debug.LogError("Missing references!");
			Destroy (this);
			return;
		}
		if(equippedWeapon == null) { 
			equippedWeapon = new Gun();
		}
		_rigidbody = GetComponent<Rigidbody2D>() as Rigidbody2D;
		if(_rigidbody == null) {
			_rigidbody = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
		}//*/

		label.text = "Ammo: " + equippedWeapon.GetCurrentAmmo();
	}

	void FixedUpdate() {
		// Physics must be done on Fixed Update]
		UpdatePosition();
	}

	void Update() {
		moveDirection = Vector3.zero;
		CheckTouches();
#if UNITY_EDITOR
		CheckDebugMovement();
		CheckDebugFire();
#endif
		//UpdatePosition();
	}

	public void ReloadWeapon() {
		equippedWeapon.Reload();
		label.text = "Ammo: " + equippedWeapon.GetCurrentAmmo();
	}

	void CheckTouches() {
		if(Input.touchCount <= 0) {
			//PrintDebug("");
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
			if(!consumed && RectTransformUtility.RectangleContainsScreenPoint(label.rectTransform, touch.position)) {
				if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					ReloadWeapon();
					consumed = true;
				}
			}
			if(!consumed && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
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
			_rigidbody.MovePosition(newLocation);
		} else {
			// Commented out until performance testing.
			// Uncomment and set Linear Drag to 0 for better performance, more sudden movement.
			//_rigidbody.MovePosition(transform.position);
		}
	}
	
	public void FireWeapon(Vector3 point) {
		// Actual firing of gun....
		equippedWeapon.Fire (transform.position, transform.rotation);
		label.text = "Ammo: " + equippedWeapon.GetCurrentAmmo();
	}

	void KillPlayer() {
		Destroy(this.gameObject);
	}

	public void TakeDamage(float damage) {
		Health -= damage;
	}

#if UNITY_EDITOR
	void CheckDebugMovement() {
		Vector3 movement = Vector3.zero;
		if(Input.GetKey(KeyCode.RightArrow)) {
			movement.x = 1;
		} else if(Input.GetKey(KeyCode.LeftArrow)) {
			movement.x = -1;
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			movement.y = 1;
		} else if(Input.GetKey(KeyCode.DownArrow)) {
			movement.y = -1;
		}
		if(moveDirection == Vector3.zero && movement != Vector3.zero)
			moveDirection = movement.normalized;
	}

	void CheckDebugFire() {
		if(Input.GetMouseButton(0)) {
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			//Debug.Log ("Mouse Pos: " + Input.mousePosition);

			bool consumed = false;
			if(RectTransformUtility.RectangleContainsScreenPoint(MovementPad, mousePosition)) {
				Vector2 localPosition = Vector2.zero;
				if(RectTransformUtility.ScreenPointToLocalPointInRectangle(MovementPad, mousePosition, null, out localPosition)) {
					Vector2 dir = localPosition - MovementPad.rect.center;
					float distance = Vector2.Distance(localPosition, MovementPad.rect.center);
					float radius = MovementPad.rect.width > MovementPad.rect.height ? MovementPad.rect.height / 2 : MovementPad.rect.width / 2;
					if(distance < radius/* && distance > radius / 10*/) {
						moveDirection = dir.normalized;
						consumed = true;
					}
				}
			}
			if(!consumed && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
				Vector3 touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));
				Vector3 diff = touchPoint - transform.position;
				diff.Normalize();
				
				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler (0f, 0f, rot_z - 90);
				FireWeapon(touchPoint);
			} 
		} else if(Input.GetMouseButtonUp(0)) {
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if(RectTransformUtility.RectangleContainsScreenPoint(label.rectTransform, mousePosition)) {
				ReloadWeapon();
			}
		}
	}
#endif
}
