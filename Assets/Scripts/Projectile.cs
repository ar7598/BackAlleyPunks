using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Projectile
// Author: Ajay Ramnarine
// Purpose: Move the projectile once it has been set active from the pool
//          Check to see whether the projectile has hit the player or the boss depending on if it has been reflected or not
//          Explode the projectile and disable it once it has returned to the pool
// Restrictions: The pool is within the ProjectilePool script
public class Projectile : MonoBehaviour
{
    public GameObject hitEffect;
    GameObject boss;
    Transform bossTransform;
    private GameManager gameManager;

    public bool isBossProj = true;
    public bool isPlayerProj = false;

    private Vector2 moveDirection;
    private float moveSpeed = 5f;
    public int damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        StartCoroutine(ProjectileDestroy());
    }

    // Method: SetMoveDirection
    // Purpose: Sets the direction for the projectile to move
    // Restrictions: None
    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    // Method: OnTriggerEnter2D
    // Purpose: Checks to see if the projectile has collided with other objects
    // Restrictions: None
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // check to see if collide with player
        PlayerController player = hitInfo.GetComponent<PlayerController>();
        BossController boss = hitInfo.GetComponent<BossController>();
        Projectile proj = hitInfo.GetComponent<Projectile>();

        // if the projectile collides with the boss or another projectile, then return
        if (proj != null)
            return;

        // if it does collide with the player and belongs to the boss then damage the player
        if (player != null && isBossProj)
        {
            // deal damage to the player if they are hit
            player.Damaged(damage);

            // reset the player's multiplier for their score
            gameManager.ResetMultiplier();

        }
        else if (player != null && isPlayerProj)
            return;

        // check to see if it collides with the boss when the player has reflected the projectile
        if (boss != null && isPlayerProj)
        {
            // deal damage to the boss
            boss.Damaged(damage);

        }
        else if (boss != null && isBossProj)
            return;

        // call the explode method to cause the projectile to be destroyed after it hits the player or surrounding area
        Explode();
    }

    // Method: Explode
    // Purpose: Cause the projectile to explode if it is hit or if it hits something
    // Restrictions: None
    public void Explode()
    {
        // upon collision, the object will play the explosion effect
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        // the effect will be destroyed after 0.25 seconds
        Destroy(effect, 0.25f);

        // the projectile will be set to false as it is returned to the pool
        gameObject.SetActive(false);

        // set isPlayerProj to false and isBossProj to true in case they have been swapped and the projectile is called again from the pool
        // this should avoid damaging the boss the next time the projectile is called
        isBossProj = true;
        isPlayerProj = false;

        // set the move speed back to its normal speed
        moveSpeed = 5f;
    }

    // Method: Reflect
    // Purpose: When hit by the player's UAttack the projectile should return to the boss
    // Restrictions: None
    public void Reflect()
    {
        // swap the booleans from isBossProj to IsPlayerProj
        isBossProj = false;
        isPlayerProj = true;

        // get the boss's current position
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        Vector2 bossPos = new Vector2(bossTransform.position.x, bossTransform.position.y);
        Vector2 newPos = (bossPos - new Vector2(transform.position.x, transform.position.y)).normalized;

        // set the move speed to a faster speed once reflected
        moveSpeed = 10f;

        // set the move direction of the projectile to the boss's position
        SetMoveDirection(newPos);
    }

    IEnumerator ProjectileDestroy()
    {
        yield return new WaitForSecondsRealtime(8f);

        Explode();
    }
}
