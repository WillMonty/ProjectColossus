using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject spine;

    GameObject model;

    PlayerMovement playerMovement;
    IWeapon weaponState;

    Animator controllerExternal;
    Animator controllerInternal;

    Vector3 rotGun;
    int dir;
    bool reloading = false;
    // Use this for initialization
    void Start ()
    {
       
        model = GetComponent<PlayerData>().model;
        playerMovement = GetComponent<PlayerMovement>();
        
        controllerExternal = model.GetComponent<Animator>();
        controllerInternal = GetComponent<PlayerData>().eyes.transform.GetChild(1).gameObject.GetComponent<Animator>();

        
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if(weaponState==null)
        {
            weaponState = GetComponent<PlayerData>().WeaponData;
            controllerExternal.SetFloat("reloadSpeed", weaponState.ReloadTime / 3f);
            controllerInternal.SetFloat("reloadSpeed", weaponState.ReloadTime / 3f);
        }

        AnimRun();
        AnimJump();
        AnimFall();
        AnimReload();
        AimGunExternal();
	}
    
    //8 direction animation based on direction int. 1-8 are directions and 0 is idle
    void AnimRun()
    {
        controllerExternal.SetInteger("direction",  playerMovement.AnimDir);

    }

    //Aim gun on external model
    void AimGunExternal()
    {
        rotGun = GetComponent<PlayerData>().eyes.transform.rotation.eulerAngles;

        rotGun.z = 0;
        rotGun.y += 40f;
        if (rotGun.x > 90)
            rotGun.x -= 360f;
        rotGun.x *= 0.85f;
        rotGun.x += 10f;

        spine.transform.rotation = Quaternion.Euler(rotGun);
    }

    //Set jump trigger in controller
    void AnimJump()
    {
        if(playerMovement.Jumped)
        {
            controllerExternal.SetTrigger("jumped");
        }
        else 
            controllerExternal.ResetTrigger("jumped");
    }

    //Set bool in controller
    void AnimFall()
    {
       controllerExternal.SetBool("inAir", playerMovement.InAir);
    }

    //Reloading
    void AnimReload()
    {
        if (reloading)
        {
            controllerInternal.SetTrigger("reload");
            controllerExternal.SetTrigger("reload");

        }

        if (weaponState.Reloading && !reloading)
        {
            reloading = true;
            StartCoroutine(ResetReload(weaponState.ReloadTime / 3f));
        }




    }

    IEnumerator ResetReload(float time)
    {

        yield return new WaitForSeconds(time);

        controllerInternal.ResetTrigger("reload");
        controllerExternal.ResetTrigger("reload");
        reloading = false;
    }
}
