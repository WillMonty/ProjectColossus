using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
		//Exception to not destroy anything on the colossus
		if(other.tag == "colossusplayer" || other.tag == "MainCamera")
		{
			return;
		}

		//Destroy/Kill anything with IHealth
		if(other.gameObject.GetComponent<IHealth>() != null)
		{
			IHealth healthInterface = other.gameObject.GetComponent<IHealth>();
			healthInterface.DamageObject(healthInterface.Health);
			return;
		}

		//Destroy any other physics based 
		if(!other.attachedRigidbody.isKinematic)
			GameObject.Destroy(other.gameObject);
    }
}
