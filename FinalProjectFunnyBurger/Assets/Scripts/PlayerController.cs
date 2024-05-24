using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5.0f;
    private CharacterController characterController;
    float horizontal;
    float vertical;
    Vector3 movement;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        
    }


    
    void FixedUpdate()
        
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 velo = characterController.velocity;
        
        if (horizontal != 0 || vertical != 0)
        {
            movement = new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;

            characterController.Move(movement);
            
            
        }


        

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
        
    }


}
