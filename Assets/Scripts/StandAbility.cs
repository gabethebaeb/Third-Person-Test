using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StandAbility : MonoBehaviour
{
    public string abilityName;
    public float cooldownTime;
    private float currentCooldown;
    protected bool isOnCooldown;

    protected StandAttack standAttack;
    protected Transform stand;
    protected Transform player;

    public abstract void ActivateAbility();

    protected virtual void Start()
    {
        standAttack = GetComponent<StandAttack>();
        stand = standAttack.stand;
        player = standAttack.player;
        currentCooldown = 0;
        isOnCooldown = false;
    }

    protected virtual void Update()
    {
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                isOnCooldown = false;
                Debug.Log("Cooldown finished"); // Added debug message
            }
        }
    }


    protected void StartCooldown()
    {
        Debug.Log("started cooldown");
        currentCooldown = cooldownTime;
        isOnCooldown = true;
    }
}

