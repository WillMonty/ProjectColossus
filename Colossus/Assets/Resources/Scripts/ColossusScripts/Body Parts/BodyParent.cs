using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParent : MonoBehaviour, IHealth {

	public ColossusManager colossus;
	public float health;
	public GameObject hitPrefab;
	public float damageMultiplier = 1f;
	public Color damageColor = Color.red;

	Material originalMat;
	Color originalColor;
	[HideInInspector]
	public Material flashMat;
	bool flashing;
	float currFlashTime;

	public float Health
	{
		get
		{
			return health;
		}
	}

	void Start()
	{
		originalMat = gameObject.GetComponent<Renderer>().material;
		originalColor = gameObject.GetComponent<Renderer>().material.color;
		flashMat = GameManagerScript.instance.flashMat;
	}

	void Update()
	{
		if(flashing)
			currFlashTime += Time.deltaTime;
			
		if(currFlashTime >= GameManagerScript.instance.damageFlashTime)
		{
			SwapAppearance(gameObject, originalMat, originalColor);
			flashing = false;
			currFlashTime = 0;
		}
	}

	public void DamageObject(float dmg)
	{
		health -= dmg;
		//if(health <= 0) body part falls off if applicable
		Instantiate(hitPrefab, gameObject.transform.position, Quaternion.identity);

		//Apply damage to Colossus
		colossus.DamageObject(dmg * damageMultiplier);

		//Damage visual
		if(GameManagerScript.instance.currentGameState == GameState.InGame)
		{
			SwapAppearance(gameObject, flashMat, damageColor);
			flashing = true;
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of damaging object
		if(collision.gameObject.GetComponent<IDamage>() != null)
		{
			//Apply damage to Colossus
			DamageObject(collision.gameObject.GetComponent<IDamage>().Damage);

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}

	public void SwapAppearance(GameObject targetObj, Material newMat, Color newColor)
	{
		targetObj.GetComponent<Renderer>().material = newMat;
		targetObj.GetComponent<Renderer>().material.color = newColor;
	}
}
