using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "resistanceplayer":
				//Kill player
				other.gameObject.GetComponent<PlayerManager>().Damage(100.0f);
                break;
            case "colossusplayer":
                //Do nothing
                break;
            case "MainCamera":
                //Do nothing
				break;
            default:
				if(!other.attachedRigidbody.isKinematic)
               		 GameObject.Destroy(other.gameObject);
                break;
        }
    }
}
