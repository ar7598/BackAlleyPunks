using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: BossController
// Author: Ajay Ramnarine
// Purpose: Contains all variables and methods associated with the movement and attacks of the boss
// Restrictions: None
public class BossController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public GameObject boss;
    private GameManager gameManager;

    // health of the boss
    public int maxHealth = 700;
    int currentHealth;

    public HealthBar healthBar;

    // parameters to perform knockback
    public float startKnockback = 0.7f;
    float currentKnockback;
    public float knockbackSpeed;
    bool isKnockback = false;
    Vector2 knockbackDirection;
    float playerXPos = 100000f;
    float playerYPos = 100000f;

    // parameters to perform attacks
    public Transform attackPoint;
    public Vector2 attackRange = new Vector2(0.5f, 0.5f);
    public int attackDamage = 15;
    public float attackKnockback = 80f;
    public int enragedAttackDamage = 30;
    public float enragedAttackKnockback = 100f;

    public Transform chargePoint;
    public Vector2 chargeRange = new Vector2(0.5f, 0.5f);
    public int chargeDamage = 40;
    public float chargeKnockback = 100f;

    public Transform headbuttPoint;
    public Vector2 headbuttRange = new Vector2(0.5f, 0.5f);
    public int headbuttDamage = 45;
    public float headbuttKnockback = 50f;

    public Transform projectilePool;

    public LayerMask playerLayer;

    // bool to check if the boss is enraged
    public bool isEnraged = false;

    // bool to make boss invulnerable while powering up
    public bool isInvulnerable = false;

    // bool to check if the boss needs to be flipped
    public bool isFlipped = false;

    // get access to variables of the player
    Rigidbody2D playerRB;
    Animator playerAnimator;

    // flipped vectors for the sprite
    Vector3 flipped = new Vector3(-1f, 1f, 1f);
    Vector3 notFlipped = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        // disable the projectile pool at the beginning of the fight so it doesn't fire right away
        GameObject.FindGameObjectWithTag("Boss").GetComponent<ProjectileFire>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // get the game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // get access to the player's health
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        // get access to the player's transform position
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        // set the currentHealth of the boss equal to the maxHealth
        currentHealth = maxHealth;

        // set the maximum value of the healthbar
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // flips the enemy image based on the position of the player
        if (playerRB.position.x > rb.position.x && !animator.GetBool("IsCharging"))
        {
            isFlipped = true;
            transform.localScale = flipped;
        }
        else if (playerRB.position.x <= rb.position.x && !animator.GetBool("IsCharging"))
        {
            isFlipped = false;
            transform.localScale = notFlipped;
        }

        if(animator.GetBool("IsCharging") && isFlipped)
        {
            transform.localScale = flipped;
        }
        else if(animator.GetBool("IsCharging") && !isFlipped)
        {
            transform.localScale = notFlipped;
        }

        // check to see if the enemy has been defeated
        if(currentHealth <= 0)
        {
            Defeated();
        }

        // knockback
        if (isKnockback)
        {
            // checks to make sure that the position of the player is only saved from the point of hitting
            if (playerXPos == 100000)
            {
                // get the x and y position of the player from the hit
                playerXPos = playerRB.position.x;
                playerYPos = playerRB.position.y;
            }

            // calculate the position that the boss will be knocked back
            if(playerXPos >= rb.position.x)
            {
                knockbackDirection.x = -1f;
            }
            else
            {
                knockbackDirection.x = 1f;
            }

            if(playerYPos >= rb.position.y)
            {
                knockbackDirection.y = -1f;
            }
            else
            {
                knockbackDirection.y = 1f; 
            }
            
            // add a velocity to the rigid body of the boss
            rb.velocity = knockbackDirection * knockbackSpeed * Time.deltaTime;

            // decrement the current knockback time
            currentKnockback -= Time.deltaTime;

            // once currentKnockback is at zero, set the knockback boolean to false
            if(currentKnockback <= 0)
            {
                rb.velocity = Vector2.zero;
                isKnockback = false;

                // set the player positions back to their original values
                playerXPos = 100000f;
                playerYPos = 100000f;
            }
        }
        
        // if the player is defeated, play the victory animation 
        if(playerAnimator.GetBool("IsDefeated"))
        {
            // set the animator bool for the player being defeated to true
            animator.SetBool("IsPlayerDefeated", true);
        }
        
    }

    // Method: Damaged
    // Purpose: Decrement the current health of the boss based on the attack from the player
    // Restrictions: None
    public void Damaged(int damage)
    {
        // if the boss is invulnerable, then he takes no damage
        if (isInvulnerable)
            return;

        // reduce health by damage taken
        currentHealth -= damage;

        // change attached slider to match current maxHealth
        healthBar.SetHealth(currentHealth);

        // set the knockback boolean to true
        isKnockback = true;

        // set the current knockback to start knockback
        currentKnockback = startKnockback;

        // check to see if the boss should be powered up
        if (currentHealth <= 350 && !isEnraged)
        {
            // set the enraged bool to true
            isEnraged = true;

            // set the animator bool for enraged to true to transition to the enraged form of the boss
            animator.SetBool("IsEnraged", true);
        }

    }

    // Method: Defeated
    // Purpose: Once the boss has been defeated the battle should end
    // Restrictions: None
    void Defeated()
    {
        Debug.Log("Boss Defeated!");

        // play the defeated animation
        animator.SetBool("IsDefeated", true);

        // disable the script of the boss
        this.enabled = false;
    }

    // Method: Attack
    // Purpose: The boss will attack and deal damage if they hit the player
    // Restrictions: None
    public void Attack()
    {
        // detect nearby player that will be damaged by the attack
        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, attackRange, 0f, playerLayer);

        if(hitPlayer != null)
        {
            if (!isEnraged)
            {
                // damage and knockback the player if they are detected
                hitPlayer.GetComponent<PlayerController>().Damaged(attackDamage);
                hitPlayer.GetComponent<PlayerController>().knockbackSpeed = attackKnockback;
            }
            else
            {
                // damage and knockback the player based on the enraged damage
                hitPlayer.GetComponent<PlayerController>().Damaged(enragedAttackDamage);
                hitPlayer.GetComponent<PlayerController>().knockbackSpeed = enragedAttackKnockback;

            }

            // reset the player's multiplier for their score
            gameManager.ResetMultiplier();
        }
    }

    // Method: Charge
    // Purpose: The boss will charge and deal damage if they hit the player
    // Restrictions: None
    public void Charge()
    {
        // detect nearby player that will be damaged by the attack
        Collider2D hitPlayer = Physics2D.OverlapBox(chargePoint.position, chargeRange, 0f, playerLayer);

        if(hitPlayer != null)
        {
            // damage and knockback the player if they are detected
            hitPlayer.GetComponent<PlayerController>().Damaged(chargeDamage);
            hitPlayer.GetComponent<PlayerController>().knockbackSpeed = chargeKnockback;

            // reset the player's multiplier for their score
            gameManager.ResetMultiplier();
        }
    }

    // Method: Headbutt
    // Purpose: The enraged boss will perform a headbutt attack that will damage the player if it connects
    // Restrictions: None
    public void Headbutt()
    {
        // detect nearby player that will be damaged by the attack
        Collider2D hitPlayer = Physics2D.OverlapBox(headbuttPoint.position, headbuttRange, 0f, playerLayer);

        if (hitPlayer != null)
        {
            // damage and knockback the player if they are detected
            hitPlayer.GetComponent<PlayerController>().Damaged(headbuttDamage);
            hitPlayer.GetComponent<PlayerController>().knockbackSpeed = headbuttKnockback;

            // reset the player's multiplier for their score
            gameManager.ResetMultiplier();
        }
    }

    // Method: OnDrawGizmosSelected
    // Purpose: Used to draw the shape of the attacks so that their hit boxes can be adjusted visually
    // Restrictions: None
    void OnDrawGizmosSelected()
    {
        if (chargePoint == null || attackPoint == null)
            return;

        // draws the hitbox for attack
        Gizmos.DrawWireCube(attackPoint.position, attackRange);

        // draws the hitbox for the charge
        Gizmos.DrawWireCube(chargePoint.position, chargeRange);

        // draws the hitbox for the headbutt
        Gizmos.DrawWireCube(headbuttPoint.position, headbuttRange);
    }
}
