using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
	private float timeBtwAttack;
	private float reloadTime = 1f;

	[SerializeField] private GameObject m_bullet;
	private float m_bulletSpawnOffset = 2;

	//[SerializeField] private Transform m_firePoint;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (timeBtwAttack <= 0 &&
			MicrophoneLevel.getInstance().getMicLoudness() > MicrophoneLevel.getInstance().m_thresholdWeak)
		{
			// Change this with the given shoot direction
			Shoot(Vector3.right);
			timeBtwAttack = reloadTime;
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
	}

	void Shoot(Vector3 direction)
	{
		Vector3 pos = transform.position;

		pos += direction * m_bulletSpawnOffset;
		GameObject bulletClone = Instantiate(m_bullet, pos, Quaternion.identity);
		bulletClone.GetComponent<Rigidbody>().velocity = direction * m_bullet.GetComponent<Bullet>().Speed;
	}


}
