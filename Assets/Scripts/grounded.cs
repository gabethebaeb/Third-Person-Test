using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grounded : MonoBehaviour
{
    CapsuleCollider capsule;
    public bool isGrounded;
    public RaycastHit groundHit;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the distance from the character controller's bottom to the ground
        float distToGround = controller.bounds.extents.y;
        // Check if the character is grounded by casting a ray at the bottom of the character controller
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
        Debug.Log(isGrounded);
    }
}