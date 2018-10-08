using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGunExternal : MonoBehaviour {

    public GameObject spine;
    public GameObject eyes;
    Vector3 rotation;



	
	void LateUpdate () {

        rotation = eyes.transform.rotation.eulerAngles;

        rotation.z = 0;
        rotation.y += 40f;
        if (rotation.x > 90)
            rotation.x -= 360f;
        rotation.x *= 0.85f;
        rotation.x += 10f;
        spine.transform.rotation = Quaternion.Euler(rotation);

    


    }
}
