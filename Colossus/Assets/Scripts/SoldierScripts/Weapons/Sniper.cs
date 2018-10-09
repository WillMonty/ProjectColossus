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
    Color trailColor;

    float count = 0;
    protected override void Start()
    {
        weaponType = Weapons.Sniper;
        base.Start();
        trail = GetComponent<LineRenderer>();
        trail.material = new Material(trailMat);
        trailColor = trail.material.color;
    }

    protected override void Shoot()
    {
        trail.material.color = trailColor;

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

        StartCoroutine(FadeTrail());
        StartCoroutine("ShootingBoolReset");
    }


    IEnumerator FadeTrail()
    {
        while (count<=2.0f)
        {
            count += Time.deltaTime;

            trail.material.color = Color.Lerp(trailColor,new Color(0,0,0,0), count);
            
            yield return null;
        }
        trail.enabled = false;
        count = 0;
    }
}
