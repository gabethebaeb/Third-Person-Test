using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    grounded grounded;
    public CharacterController controller;
    public Transform cam;

    [SerializeField]
    public float moveSpeed = 9f;
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    [SerializeField]
    private float jumpForce = 3.5f;
    [SerializeField]
    private float gravity = 9.81f;


    float turnSmoothVelocity;
    public Vector3 playerVelocity;

    public bool isSprinting;
    public float currentFOV;
    public float transitionSpeed;
    public float baseFOV;
    public float sprintModifier;

    // Combat mode
    public bool combatMode = false;

    private void Start()
    {
        currentFOV = Camera.main.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        grounded = GetComponentInChildren<grounded>();
        baseFOV = 40f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            combatMode = !combatMode;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (combatMode)
        {
            Vector3 moveDirection = cam.transform.TransformDirection(new Vector3(horizontal, 0, vertical));
            moveDirection.y = 0;
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

            // Rotate the player to face the camera's direction
            Vector3 cameraDirection = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
        }
        else
        {
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            }
        }

        if (grounded.isGrounded && playerVelocity.y < 0)
        {
            //playerVelocity.y = -0.5f;
        }

        if (grounded.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            Sprint();
            currentFOV = Mathf.Lerp(currentFOV, 100f, Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentFOV = Mathf.Lerp(currentFOV, baseFOV, Time.deltaTime * transitionSpeed);
            isSprinting = false;
            moveSpeed = 9f;
        }

        Camera.main.fieldOfView = currentFOV;

        void Sprint()
        {
            moveSpeed = 9f * sprintModifier;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
