using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
	[SerializeField]
	private GameObject BulletPrefab;

	[SerializeField]
	private float BulletSpeed;

	private bool fireContinously;

	private void FireBullet()
	{
		GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
		Rigidbody2D rigidbody = Bullet.GetComponent<Rigidbody2D>();

		rigidbody.velocity = BulletSpeed * transform.up;
	}

	public void OnPlayerInputShoot(InputAction.CallbackContext context)
	{
		if (!context.performed)
			return;

		FireBullet();
		Debug.Log($"Shoot! {Time.time}", gameObject);
	}
}
