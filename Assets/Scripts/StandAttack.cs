using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAttack : MonoBehaviour
{
    public Transform stand;
    public Transform player;
    public Animator animator;
    public Bounce bounce;

    public List<StandAbility> abilities;

    private Vector3 playerPos;
    private Vector3 playerDirection;

    public float speed;
    public float timeBetweenBasicAttack = .5f;
    public float cooldownTime = 1f;
    private float timeSinceClick;
    public int attackCount;


    public bool isFront;
    public bool isMoving;
    public bool isIdle;
    public bool isAttacking;
    public bool isPowerPunching;
    public bool attackCooldown;
    

    [SerializeField]
    public float basicAttackSpeed = .3f;
    public float basicAttackDamage = .5f;
    public float basicAttackDistance = 1.5f;

    [SerializeField]
    public float powerPunchSpeed = .5f;
    public float powerPunchDamage = 10f;
    public float powerPunchDistance = 1.5f;


    private void Awake()
    {
        player = FindObjectOfType<CharacterController>().transform;
        animator = GetComponent<Animator>();
        abilities = new List<StandAbility>(GetComponents<StandAbility>());
        stand = this.transform;
        bounce = GetComponent<Bounce>();


        basicAttackDistance = 1.5f;
        basicAttackSpeed = .3f;
        basicAttackDamage = .5f;

        powerPunchSpeed = .5f;
        powerPunchDamage = 10f;
        powerPunchDistance = 1.5f;


        timeSinceClick = 0f;

        isFront = false;
        isMoving = false;
        isIdle = true;
        isAttacking = false;
        isPowerPunching = false;

    }
    private void Start()
    {


    }

    private void Update()
    {
        playerPos = player.transform.position;
        playerDirection = player.transform.forward;
        if (stand != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isMoving && !isFront)
                {
                    StartCoroutine(MoveToFront(basicAttackDistance, basicAttackSpeed, false));
                }
                else if (isFront)
                {
                    timeSinceClick = 0f;
                    if (timeSinceClick >= cooldownTime)
                    {
                        StartCoroutine(MoveToBack());
                    }
                }
                if (isFront || isMoving)
                {
                    if (!attackCooldown)
                    {
                        attackCount++;
                        attackCooldown = true;
                        StartCoroutine(EnemyAttack(basicAttackDamage));
                    }
                }
            }
            else if (isFront)
            {
                timeSinceClick += Time.deltaTime;
                if (timeSinceClick >= cooldownTime)
                {
                    StartCoroutine(MoveToBack());
                }
            }

            if (isFront)
            {
                transform.parent.GetComponent<ThirdPersonMovement>().moveSpeed = 3f;
            }

            if (attackCount >= 6)
            {
                StartCoroutine(MoveToBack());
                
                attackCount = 0;
            }

            if (Input.GetKeyDown(KeyCode.R) && !isMoving && !isPowerPunching)
            {
                StartCoroutine(PowerPunch());
            }
        }
    }

    private IEnumerator PowerPunch()
    {
        
        // Reset the animation state before playing it again      
        isPowerPunching = true;
        animator.Play("PowerPunch");
        yield return StartCoroutine(MoveToFront(powerPunchDistance, powerPunchSpeed, true));

        yield return new WaitForSeconds(.15f);

        // Assuming EnemyAttack takes a float parameter as the damage value
        yield return StartCoroutine(EnemyAttack(powerPunchDamage));

        // Add a delay before moving back, for example, you can use WaitForSeconds
        yield return new WaitForSeconds(.2f); // Adjust this value to change the delay

        StartCoroutine(MoveToBack());
        animator.Play("Idle");
        isPowerPunching = false;
    }



    public IEnumerator EnemyAttack(float damageAmount)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            yield return new WaitForSeconds(.15f);

            RaycastHit hit;
            if (Physics.Raycast(stand.position, stand.forward, out hit, 1.5f))
            {
                GameObject hitGameObject = hit.collider.gameObject;
                if (hitGameObject != null)
                {
                    Health health = hitGameObject.GetComponent<Health>();
                    if (health != null)
                    {
                        // Add a delay before dealing damage           
                        health.TakeDamage(damageAmount);
                        Debug.Log("hit");
                    }
                }
            }
            yield return new WaitForSeconds(timeBetweenBasicAttack);
            attackCooldown = false;
            isAttacking = false;
        }
    }

    public IEnumerator MoveToFront(float distance, float speed, bool stayInFront)
    {
        isMoving = true;
        bounce.enabled = false;

        float duration = 0.1f;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            Vector3 playerPos = player.position;
            Quaternion playerRotation = player.rotation;

            Vector3 spawnPos = playerPos + playerRotation * (Vector3.forward * distance);
            spawnPos.y = playerPos.y + 1.5f;

            stand.position = Vector3.Lerp(stand.position, spawnPos, ((Time.time - startTime) / duration) * speed);
            stand.rotation = playerRotation;

            yield return new WaitForEndOfFrame();

        }

        // Ensure the stand reaches the final position
        Vector3 finalPlayerPos = player.position;
        Quaternion finalPlayerRotation = player.rotation;
        Vector3 finalSpawnPos = finalPlayerPos + finalPlayerRotation * (Vector3.forward * distance);
        finalSpawnPos.y = finalPlayerPos.y + 1.5f;

        stand.position = finalSpawnPos;
        stand.rotation = finalPlayerRotation;

        isMoving = false;
        isFront = true;

        if (stayInFront)
        {
            isFront = false; // Reset the isFront flag
        }
        else
        {
            timeSinceClick = 0f;
        }
    }


    public IEnumerator MoveToBack()
    {
        isMoving = true;
        isFront = false;
        

        Vector3 originalPos = new Vector3(-1, 1.5f, -1.5f);

        float duration = 0.1f;
        float startTime = Time.time;

        Vector3 standAttackVelocity = Vector3.zero; // Add this line to create a new velocity variable

        while (Time.time < startTime + duration)
        {
            stand.localPosition = Vector3.SmoothDamp(stand.localPosition, originalPos, ref standAttackVelocity, duration * speed);
            bounce.enabled = true;
            yield return new WaitForEndOfFrame();
        }
        attackCount = 0;
        isMoving = false;
        timeSinceClick = 0f;
        stand.localPosition = originalPos;
        

    }

}


