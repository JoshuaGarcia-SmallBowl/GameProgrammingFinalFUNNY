using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5.0f;
    private CharacterController characterController;
    float horizontal;
    float vertical;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        
    }


    // Update is called once per frame
    void Update()
        
    {
        //movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * speed;
        characterController.Move(movement);
       

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
    private void LookAtMouse()
    {
        
    }

}
