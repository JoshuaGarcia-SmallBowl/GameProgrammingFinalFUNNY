using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileShoot : MonoBehaviour
{
    private float atkStart;
    private bool held;

    private bool firing = false;
    private bool burning = false;
    private int ammo = 7;
    public int maxAmmo = 7;
    public float firingRate = 0.3f;
    private bool reloading = false;
    public float reloadRate = 0.5f;
    

    private PlayerController playerController;
    private bool frozen = true;
    
    public GameObject firePro;
    public GameObject iceProSmall;
    public GameObject iceProMedium;
    public GameObject iceProLarge;

    public TextMeshProUGUI ammoTMP;
    public TextMeshProUGUI chargeTMP;
    void Start()
    {
        ammo = maxAmmo;
        playerController = GetComponent<PlayerController>();
        ammoTMP.text = (ammo + " / " + maxAmmo);
    }

    void Update()
    {
        //Set the player as frozen or burning
        if (playerController.heat <= 20)
        {
            frozen = true;
            chargeTMP.gameObject.SetActive(true);
        }
        else if(playerController.heat >= 80)
        {
            burning = true;
            ammoTMP.gameObject.SetActive(true);
        }
        else
        {
            frozen = false;
            burning = false;
            ammoTMP.gameObject.SetActive(false);
            chargeTMP.gameObject.SetActive(false);
        }

        if (!firing && !reloading)
        {
            StartCoroutine(reload());
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
                            if (ammo > 0)
                            {
                                StartCoroutine(fireShoot());
                                
                            }

                        }
                    }
                }

            }
            else if (frozen)
            {
                float chargeTime = Time.time - atkStart;
                if (chargeTime > 1f)
                {
                    chargeTMP.text = "Charge: 3";
                }
                else if (chargeTime > 0.6f)
                {
                    chargeTMP.text = "Charge: 2";
                }
                else if (chargeTime > 0.2f)
                {
                    chargeTMP.text = "Charge: 1";
                }
                else
                {
                    chargeTMP.text = "Charge: 0";
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
                if (frozen)
                {
                    float heldTime = Time.time - atkStart;
                    if (heldTime > 1f)
                    {
                        Instantiate(iceProLarge, new(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
                        chargeTMP.text = "Charge: 0";
                    }
                    else if (heldTime > 0.6f)
                    {
                        Instantiate(iceProMedium, new(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
                        chargeTMP.text = "Charge: 0";
                    }
                    else if (heldTime > 0.2f)
                    {
                        Instantiate(iceProSmall, new(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
                        chargeTMP.text = "Charge: 0";
                    }
                    else
                    {
                        Debug.Log("Melee");
                    }
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
            
            firing = true;
        
            ammo--;
            ammoTMP.text = (ammo + " / " + maxAmmo);
        Instantiate(firePro, new(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
            Debug.Log("Ammo: " + ammo);
            yield return new WaitForSeconds(firingRate);
        firing = false;
        
        
    }

    System.Collections.IEnumerator reload()
    {
        if (ammo < maxAmmo)
        {
                     
            reloading = true;
            
            Debug.Log("Ammo: " + ammo);

            yield return new WaitForSeconds(reloadRate);
            if (!firing)
            {
                ammo++;
                ammoTMP.text = (ammo + " / " + maxAmmo);
            }
            
            reloading = false;
        }
            
        

    }

    


}
