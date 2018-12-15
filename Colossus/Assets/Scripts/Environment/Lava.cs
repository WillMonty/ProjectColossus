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
			
		if(other.gameObject.tag == "ragdoll")
			GameObject.Destroy(other.gameObject);

		//Destroy throwables, but detach from Colossus if they are holding them
		if(other.gameObject.tag == "throwable")
		{
			if(other.gameObject.transform.parent != null)
			{
				Valve.VR.InteractionSystem.Hand handHold = other.gameObject.transform.parent.gameObject.GetComponent<Valve.VR.InteractionSystem.Hand>();
				if(handHold != null)
				{
					handHold.DetachObject(other.gameObject, true);
					GameObject.Destroy(other.gameObject);
				}	
			}
			else
			{
				GameObject.Destroy(other.gameObject);
			}
		}
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
