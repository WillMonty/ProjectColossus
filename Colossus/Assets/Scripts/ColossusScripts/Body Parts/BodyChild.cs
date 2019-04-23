using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyChild : MonoBehaviour, IHealth {

	public BodyParent parent;

	Shader originalShader;
	bool flashing;
	float currFlashTime;

	public float Health
	{
		get
		{
			return parent.Health;
		}
	}

	public void DamageObject(float dmg)
	{
		parent.DamageObject(dmg);
		parent.SwapAppearance(gameObject, GameManagerScript.instance.damageShader, parent.damageColor);
		flashing = true;

		//Damage visual
		if(GameManagerScript.instance.currentGameState == GameState.InGame)
		{
			parent.SwapAppearance(gameObject, GameManagerScript.instance.damageShader, parent.damageColor);
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

	void Start()
	{
		originalShader = gameObject.GetComponent<Renderer>().material.shader;
	}

	void Update()
	{
		if(flashing)
			currFlashTime += Time.deltaTime;

		if(currFlashTime >= GameManagerScript.instance.damageFlashTime)
		{
			parent.SwapAppearance(gameObject, originalShader, Color.white);
			currFlashTime = 0;
			flashing = false;
		}
	}
}
