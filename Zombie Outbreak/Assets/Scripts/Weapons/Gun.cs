using UnityEngine;
using System.Collections;
using System.IO;

public class Gun : Weapon {
	protected int ammo = 0;
	protected int maxAmmo = 10;
	protected string name = "weapon";
	protected int damage = 1;
	protected int knockback = 0;
	protected float fireRate = 5.0f; // in bullets per second
	protected float range = 100;

	protected GameObject _Bullet;
	protected bool initialized = false;

	protected float fireTime = 0;

	protected float reloadTime = 0;
	protected float maxReloadTime = 0;

	public Gun() {
		Initialize();
	}

	void Initialize() {
		_Bullet = Resources.Load (Constants.Strings.BULLET_PREFAB_PATH) as GameObject;
		if(_Bullet == null) {
			Debug.LogError("Could not find bullet!" + Constants.Strings.BULLET_PREFAB_PATH);
		}
		ammo = maxAmmo;
		initialized = true;
	}

	public int GetDamage() {
		return damage;
	}

	public int GetKnockback() {
		return knockback;
	}

	public float GetRange() {
		return range;
	}

	#region Weapon implementation
	public bool Fire (Vector3 position, Quaternion rotation)
	{
		if(!initialized) Initialize();
		if(ammo > 0 && Time.time - fireTime >= (1 / fireRate)) {
			Debug.Log ("Firing.");
			GameObject bullet = GameObject.Instantiate(_Bullet, position, rotation) as GameObject;
			Bullet b = bullet.GetComponent<Bullet>() as Bullet;
			b.source = this;
			fireTime = Time.time;
			ammo--;
			return true;
		}
		return false;
	}

	public void Reload ()
	{
		if(!initialized) Initialize();
		ammo = maxAmmo;
	}

	public int GetCurrentAmmo ()
	{
		if(!initialized) Initialize();
		return ammo;
	}

	public string GetWeaponName ()
	{
		if(!initialized) Initialize();
		return name;
	}
	#endregion
}
