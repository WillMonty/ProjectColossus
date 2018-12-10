using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerpscript : MonoBehaviour
{
    public GameObject moveThing;
    Vector3 initPos;
    Vector3 initRot;
    float t = 0;

    private void Start()
    {
        initPos = moveThing.transform.position;
        initRot = moveThing.transform.eulerAngles;
    }


    void Update()
    {
        t += 0.001f;



        if (t <= 1)
        {
            moveThing.transform.position = Vector3.Lerp(initPos, transform.position, t);
            moveThing.transform.rotation = Quaternion.Euler(Vector3.Lerp(initRot, transform.eulerAngles, t));
        }
    }

}
