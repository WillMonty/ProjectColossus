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
                //Kill him or something
                break;
            case "colossusplayer":
                //Do nothing
                break;
            case "MainCamera":
                //Do nothing
            default:
                GameObject.Destroy(other.gameObject);
                break;
        }
    }
}
