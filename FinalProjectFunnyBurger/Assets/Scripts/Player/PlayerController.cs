using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{ 
    //movement variables
    float horizontal;
    float vertical;
    Vector3 movement;
    Vector3 targetPosition;

    //materials
    public Material fireMat;
    public Material defaultMat;
    public Material iceMat;

    //External references
    private CharacterController characterController;
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private EAbilities abilities;
    private GameManager gameManager;
    public GameObject manajero;

    //player characterstics
    public float speed = 5.0f;
    private int health = 100;
    public bool movable = false;
    public float heat;
    private bool hurtable = true;

    //ability cooldown
    private float CDStart;
    private float CDEndTime= 8.0f;
    private bool onCD;

    //UI
    public TextMeshProUGUI CDtext;
    public GameObject AbilE;
    public TextMeshProUGUI healthText;

    
    private void Start()
    {

        //get every reference
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();      
        abilities = GetComponent<EAbilities>();
        gameManager = manajero.GetComponent<GameManager>();

        //set heat bar to current heat
        heatChangePC(heat);
    }

    void FixedUpdate()
    {
        //movement
        if (movable)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");            

            //check if any input to move the player is being done, if not: make them idle
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
        //check if the ability is on cooldown
        if (onCD)
        {
            //run a timer for CDEndTime, disabling cooldown when it runs out
            float CDTime = Time.time - CDStart;
            float displayCD = CDEndTime - CDTime;

            //round the cooldown to one decimal place for the display
            displayCD = Mathf.Round(displayCD * 10.0f) * 0.1f;
            CDtext.text = ("" + displayCD);

            if (CDTime > CDEndTime)
            {
                onCD = false;
                CDtext.gameObject.SetActive(false);
            }
        }
        
        
        if (movable)
        {
            //Rotation 
            //make a ray to the mousePosition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            if (Physics.Raycast(ray, out hit))
            {
                //get the difference of the player and the ray's position, then look torward it
                targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);

            }

            //Ability
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!onCD)
                {
                    //use fire or ice ability based on heat, call on EAbilities to use them
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


    //called by enemies when they hit the player
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
                hurtable = false;
                healthText.text = ("Dead lol");
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

    
    //Used for animations
    public void damageEnd()
    {
        animator.SetBool("Damaged", false);
    }

    //called by other scripts to make the player immobile
    public void movability(bool immov)
    {
        if (!immov)
        {
            movable = false;
            hurtable = false;
        }
        else
        {
            movable = true;
            hurtable = true;
        }
    }
    

    //called by other scripts to update the player's heat
    public void heatChangePC(float heaty)
    {
        //change the player's material depending on their heat, while also making the E icon show up for their ability
        heat = heaty;
        if (heat <= 20)
        {
            meshRenderer.material = iceMat;
            AbilE.SetActive(true);
        }
        else if (heat >= 80)
        {
            meshRenderer.material = fireMat;
            AbilE.SetActive(true);
        }
        else
        {
            meshRenderer.material = defaultMat;
            AbilE.SetActive(false);
        }
    }

}
