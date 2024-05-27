using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    private float atkStart;
    private bool held;

    private bool firing = false;
    private bool burning = true;
    private int ammo = 7;
    public int maxAmmo = 7;
    public float firingRate = 0.3f;
    private bool ammoFull = true;


    private bool frozen;
    public GameObject firePro;
    void Start()
    {
        ammo = maxAmmo;
    }

    void Update()
    {
        if (ammo == maxAmmo)
        {
            ammoFull = true;
            StopCoroutine(reload());
        }
        //attacking
        if (Input.GetMouseButtonDown(0))
        {
            held = true;
            atkStart = Time.time;

        }
        if (Input.GetMouseButton(0))
        {
            if (burning)
            {
                if (!firing)
                {
                    if (held)
                    {
                        float heldTime2 = Time.time - atkStart;
                        if (heldTime2 > 0.2f)
                        {
                            if (ammoFull)
                            {
                                StartCoroutine(fireShoot());
                                firing = true;
                            }

                        }
                    }
                }

            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            /*
            if (firing)
            {
                ceaseFire();
            }
            */
            if (held)
            {
                float heldTime = Time.time - atkStart;
                if (heldTime > 0.2f)
                {
                    Debug.Log("Projectile");
                }
                else
                {
                    Debug.Log("Melee");



                }
            }
            held = false;
        }
    }

    System.Collections.IEnumerator fireShoot()
    {
        ammoFull = false;
        for (int i = ammo; i > 0; i--)
        {
            ammo--;
            Instantiate(firePro, new(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
            Debug.Log("Ammo: " + ammo);
            yield return new WaitForSeconds(firingRate);
        }
        ceaseFire();
    }

    System.Collections.IEnumerator reload()
    {
        for (int i = ammo; i < maxAmmo; i++)
        {
            ammo++;
            Debug.Log("Ammo: " + ammo);

            yield return new WaitForSeconds(0.4f);
        }

    }

    private void ceaseFire()
    {
        StopCoroutine(fireShoot());
        ammoFull = false;
        StartCoroutine(reload());
        firing = false;

    }


}
