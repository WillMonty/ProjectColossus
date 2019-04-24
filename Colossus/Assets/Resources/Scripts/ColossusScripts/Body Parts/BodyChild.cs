using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyChild : MonoBehaviour, IHealth {

	public BodyParent parent;

	Material originalMat;
	Color originalColor;
	bool flashing;
	float currFlashTime;

	public float Health
	{
		get
		{
			return parent.Health;
		}
	}

	void Start()
	{
		originalMat = gameObject.GetComponent<Renderer>().material;
		originalColor = gameObject.GetComponent<Renderer>().material.color;
	}

	void Update()
	{
		if(flashing)
			currFlashTime += Time.deltaTime;

		if(currFlashTime >= GameManagerScript.instance.damageFlashTime)
		{
			parent.SwapAppearance(gameObject, originalMat, originalColor);
			currFlashTime = 0;
			flashing = false;
		}
	}

	public void DamageObject(float dmg)
	{
		parent.DamageObject(dmg);
		flashing = true;

		//Damage visual
		if(GameManagerScript.instance.currentGameState == GameState.InGame)
		{
			parent.SwapAppearance(gameObject, parent.flashMat, parent.damageColor);
			flashing = true;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "projectile")
		{
			parent.OnCollisionEnter(collision);
		}

	}
}
