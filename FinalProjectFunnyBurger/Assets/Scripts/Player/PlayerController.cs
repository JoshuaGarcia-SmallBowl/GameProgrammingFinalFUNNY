using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;
    private CharacterController characterController;
    float horizontal;
    float vertical;
    Vector3 movement;

    public Material fireMat;
    public Material defaultMat;
    public Material iceMat;

    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private EAbilities abilities;
    
    private int health = 100;
    private bool movable = true;
    public float heat;

    Vector3 targetPosition;

    

    

    private bool hurtable = true;

    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        heatChangePC(heat);
        abilities = GetComponent<EAbilities>();
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
        //rotation
        if (movable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);

            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
           
                if (heat >= 80)
                {
                abilities.spawnProj(targetPosition, transform.position);
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

    

    public void damageEnd()
    {
        animator.SetBool("Damaged", false);
    }

    public void movability(bool immov)
    {
        if (!immov)
        {
            movable = false;
        }
        else
        {
            movable = true;
        }
    }

    public void heatChangePC(float heaty)
    {
        heat = heaty;
        if (heat <= 20)
        {
            meshRenderer.material = iceMat;
        }
        else if (heat >= 80)
        {
            meshRenderer.material = fireMat;
        }
        else
        {
            meshRenderer.material = defaultMat;
        }
    }

}
