using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCam;
    public float walkS = 6f;
    public float runS = 12f;
    public float jumpP = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public bool movable = true;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool run = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = movable ? (run ? runS : walkS) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = movable ? (run ? runS : walkS) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //Jump
        if (Input.GetButton("Jump") && movable && characterController.isGrounded){
            moveDirection.y = jumpP;
        }
        else
        {
            moveDirection.y = movementDirectionY;

        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Rotation
        characterController.Move(moveDirection * Time.deltaTime);
         if (movable)
        {
            rotationX *= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
