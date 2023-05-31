using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandMovement : MonoBehaviour
{
    public Transform stand;
    public Transform player;
    public float followSpeed = 0.5f;
    public float followDelay = 0.3f;
    public float maxDistance = 10f;

    public Vector3 standRestPosition;

    private Vector3 standVelocity = Vector3.zero;
    public float minMovement = 0.1f;


    public Vector3 playerPosition;
    public Vector3 prevPosition;

    private void Awake()
    {
        player = FindObjectOfType<CharacterController>().transform;
        stand = GetComponent<StandAttack>().transform;
        Vector3 standVelocity = Vector3.zero;
    }



    void Update()
    {
        Vector3 standPosition = stand.position;
        Vector3 distance = playerPosition - standPosition;

        // Check if the player has moved a certain amount
        if (Vector3.Distance(prevPosition, playerPosition) > minMovement)
        {
            // Reset the standVelocity
            standVelocity = Vector3.zero;

            // Check if the distance between the stand and player is greater than the maxDistance
            if (distance.magnitude > maxDistance)
            {
                 standPosition = Vector3.SmoothDamp(standPosition, playerPosition, ref standVelocity, followDelay, followSpeed);
                 stand.position = standPosition;
            }
        }
    }
}



