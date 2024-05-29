using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

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
    private GameManager gameManager;
    
    private int health = 100;
    private bool movable = false;
    public float heat;

    Vector3 targetPosition;

    private float CDStart;
    private float CDEndTime= 8.0f;
    private bool onCD;

    public TextMeshProUGUI CDtext;

    public GameObject manajero;

    public TextMeshProUGUI healthText;
    private bool hurtable = true;

    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        heatChangePC(heat);
        abilities = GetComponent<EAbilities>();
        gameManager = manajero.GetComponent<GameManager>();
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
        if (onCD)
        {
            float CDTime = Time.time - CDStart;
            float displayCD = CDEndTime - CDTime;
            displayCD = Mathf.Round(displayCD * 10.0f) * 0.1f;
            CDtext.text = ("" + displayCD);
            if (CDTime > CDEndTime)
            {
                onCD = false;
                CDtext.gameObject.SetActive(false);
            }
        }
        
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!onCD)
                {
                    if (heat >= 80)
                    {
                        abilities.spawnProj(targetPosition, transform.position);
                        CDStart = Time.time;
                        onCD = true;
                        CDtext.gameObject.SetActive(true);
                    }
                    else if (heat <= 20)
                    {
                        abilities.iceProj(transform.position, transform.rotation);
                        CDStart = Time.time;
                        onCD = true;
                        CDtext.gameObject.SetActive(true);
                    }
                }
                


            }
        }
        
        

    }

     public void takeDamage(int damage)
    {
        if (hurtable)
        {
            animator.SetBool("Damaged", true);
            health -= damage;
            healthText.text = (health + "/100");

            if (health <= 0)
            {
                animator.SetBool("Dead", true);
                movable = false;
                gameManager.GameOver();
                
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
