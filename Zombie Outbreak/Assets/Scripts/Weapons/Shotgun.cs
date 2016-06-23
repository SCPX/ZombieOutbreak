using UnityEngine;
using System.Collections;

public class Shotgun : Gun {

	public override bool Fire (Vector3 position, Quaternion rotation) {
		return true;
	}
}
