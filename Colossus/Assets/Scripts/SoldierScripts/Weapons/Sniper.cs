using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : GunBase {

    Ray shot;
    float damage;
    Vector3 shotDist;
    RaycastHit shotData;
    public Material trailMat;
    LineRenderer trail;
    protected override void Start()
    {
        weaponType = Weapons.Sniper;
        base.Start();
        trail = GetComponent<LineRenderer>();
        trail.material = trailMat;
    }

    protected override void Shoot()
    {
        shot.origin = transform.position;
        shot.direction = transform.forward;
        if(Physics.Raycast(shot,out shotData))
        {
            if (shotData.collider.gameObject.GetComponent<IHealth>() != null)
                shotData.collider.gameObject.GetComponent<IHealth>().DamageObject(damage);

            shotDist = shot.origin + shotData.distance * shot.direction;
        }
        else
        {
            shotDist = shot.origin + shot.direction * 10f;

        }
        trail.enabled = true;
        trail.SetPosition(0, shot.origin);
        trail.SetPosition(1, shotDist );
        bulletsInMag--;

        justShot = true;
        StartCoroutine("ShootingBoolReset");
    }
}
