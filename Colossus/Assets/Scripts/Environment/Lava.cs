using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour
{
	public float damagePerTick;

    void OnTriggerEnter(Collider other)
    {
		//Exception to not hurt anything on the colossus
		if(other.tag == "colossusplayer" || other.tag == "MainCamera")
		{
			return;
		}

		//Hurt anything with IHealth
		if(other.gameObject.GetComponent<IHealth>() != null)
		{
			other.gameObject.GetComponent<IHealth>().DamageObject(damagePerTick);
			return;
		}

		//Destroy any other physics based 
		if(!other.attachedRigidbody.isKinematic)
			GameObject.Destroy(other.gameObject);
    }

	void OnTriggerStay(Collider other)
	{
		//Exception to not hurt anything on the colossus
		if(other.tag == "colossusplayer" || other.tag == "MainCamera")
		{
			return;
		}

		//Hurt anything with IHealth
		if(other.gameObject.GetComponent<IHealth>() != null)
		{
			other.gameObject.GetComponent<IHealth>().DamageObject(damagePerTick);
		}
	}
}
