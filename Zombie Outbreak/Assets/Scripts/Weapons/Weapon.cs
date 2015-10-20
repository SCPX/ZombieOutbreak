using UnityEngine;

public interface Weapon {
	/// <summary>
	/// Fires the weapon.
	/// Returns true is successfully fire. False if the weapon failed to fire. (e.g. Empty ammo)
	/// </summary>
	bool Fire(Vector3 position, Quaternion rotation);

	/// <summary>
	/// Refills the weapon to max ammo capacity.
	/// </summary>
	void Reload();

	/// <summary>
	/// Gets the amount of ammo in the current clip.
	/// </summary>
	int GetCurrentAmmo();

	/// <summary>
	/// Returns the string name of the weapon.
	/// </summary>
	string GetWeaponName();
}
