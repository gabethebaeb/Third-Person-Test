using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityBarrage : StandAbility
{
    private float barrageStartTime;
    public KeyCode barrageKey = KeyCode.E;

    public bool isBarraging = false;

    public float barrageDamage = .5f;
    public float barrageDuration = 5f;
    public float barrageTimeBetweenHits = 0.01f;


    private void Update()
    {
        base.Update(); // Make sure to call the base.Update() to handle the cooldown logic

        if (Input.GetKeyDown(barrageKey) && !isBarraging && !isOnCooldown)
        {
            StartCoroutine(ActivateBarrage());
        }
        else if (Input.GetKeyUp(barrageKey) && isBarraging)
        {
            StopBarrage();
            StartCooldown();
        }
    }


    IEnumerator ActivateBarrage()
    {
        isBarraging = true;
        isOnCooldown = false; // Add this line to reset the cooldown flag
        barrageStartTime = Time.time;

        standAttack.StartCoroutine(standAttack.MoveToFront(1.5f, .3f, true));

        yield return new WaitForSeconds(0.1f); //wait for stand to get to the front before attacking

        if (isBarraging)
        {
            //standAttack.isFront = true;
        }

        while (isBarraging)
        {
            if (Time.time - barrageStartTime >= barrageDuration)
            {
                // Stop the barrage if it has been going on for more than the allowed duration
                StopBarrage();
                StartCooldown();

                break;
            }
            
            Debug.Log("MUDA");
            standAttack.StartCoroutine(standAttack.EnemyAttack(barrageDamage));
            yield return new WaitForSeconds(barrageTimeBetweenHits);
        }
    }


    void StopBarrage()
    {
        if (isBarraging)
        {
            isBarraging = false;
            standAttack.StartCoroutine(standAttack.MoveToBack());
        }
    }

    public override void ActivateAbility()
    {
        // You can implement the ability activation here if needed.
    }
}
