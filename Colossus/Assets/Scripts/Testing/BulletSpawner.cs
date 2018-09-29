﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

	public GameObject bullet;

	[Range(0.1f, 5f)]
	public float timeBetweenShots;
	private float currentTime;

	public float shotForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentTime += Time.deltaTime;

		if(currentTime > timeBetweenShots) Shoot();
	}

	void Shoot()
	{
		SetupShot(Instantiate(bullet, transform.position, transform.localRotation));

		currentTime = 0.0f;
	}

	void SetupShot(GameObject newBullet)
	{
		newBullet.GetComponent<Rigidbody>().AddForce(transform.localPosition.normalized * shotForce);
		TrashCollector.AddRubbishToList(newBullet);
	}

	// Show the force and direction it spawns bullets
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * shotForce);
	}
}