using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;
    private CharacterController characterController;
    float horizontal;
    float vertical;
    Vector3 movement;

    private Animator animator;
    private int health = 100;
    private bool movable = true;

    private float atkStart;
    private bool held;

    private bool firing = false;
    private bool burning = true;
    private int ammo = 7;
    public int maxAmmo = 7;
    public float firingRate = 0.3f;
    private bool ammoFull = true;


    private bool frozen;

    

    private bool hurtable = true;

    //Projectiles
    public GameObject firePro;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ammo = maxAmmo;
    }

    void FixedUpdate()

    {
        if (movable)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            Vector3 velo = characterController.velocity;

            if (horizontal != 0 || vertical != 0)
            {
                movement = new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;

                characterController.Move(movement);
                animator.SetBool("Moving", true);


            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }



    }

    private void Update()
    {
        if (ammo == maxAmmo)
        {
            ammoFull = true;
            StopCoroutine(reload());
        }
        if (movable)
        {
            //rotation
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);

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
        


    }

     public void takeDamage(int damage)
    {
        if (hurtable)
        {
            animator.SetBool("Damaged", true);
            health -= damage;
            Debug.Log("Health" + health);
            if (health <= 0)
            {
                animator.SetBool("Dead", true);
                movable = false;
            }
            StartCoroutine(Invincibility());
        }
        
        
    }

    System.Collections.IEnumerator Invincibility()
    {
        hurtable = false;
        yield return new WaitForSeconds(1);
        hurtable = true;      
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

    public void damageEnd()
    {
        animator.SetBool("Damaged", false);
    }



}
